using AutoMapper;
using Confluent.Kafka;
using E_Book_Recommendation.Business.Logic.Implementation;
using E_Book_Recommendation.Business.Logic.Interface;
using E_Book_Recommendation.Common.Mapping;
using E_Book_Recommendation.CronJob.Jobs;
using E_Book_Recommendation.Data.DataAccess;
using E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces;
using E_Book_Recommendation.Data.UnitOfWork_;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Load configuration
config.SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false);

// Retrieve configuration values
int UDPRemotePort = config.GetValue<int>("AppSettings:UDPRemotePort");
string UDPRemoteAddress = config.GetValue<string>("AppSettings:UDPRemoteAddress");
string LogUrl = config.GetValue<string>("AppSettings:LogUrl");
bool EnableUDP = config.GetValue<bool>("AppSettings:EnableUDP");
bool EnableSEQ = config.GetValue<bool>("AppSettings:EnableSEQ");

// Configure Serilog
var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.WithProperty("Polaris Digital Payment Service CronJob", "Payments.CronJob")
    .Enrich.FromLogContext()
    .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore.Infrastructure"))
    .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore.Database.Command"))
    .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor"))
    .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Authorization.DefaultAuthorizationService"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.SqlServer.ExpirationManager"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.SqlServer.SqlServerObjectsInstaller"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.Server.BackgroundServerProcess"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.Server.ServerWatchdog"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.Server.ServerHeartbeatProcess"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.Processing.BackgroundExecution"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.SqlServer.CountersAggregator"))
    .Filter.ByExcluding(Matching.FromSource("Hangfire.BackgroundJobServer"));

if (EnableUDP)
{
    loggerConfig.WriteTo.Udp(UDPRemoteAddress, UDPRemotePort, AddressFamily.InterNetwork);
}

if (EnableSEQ)
{
    loggerConfig.WriteTo.Seq(LogUrl);
}
else
{
    loggerConfig.WriteTo.Console();
               
}

Log.Logger = loggerConfig.CreateLogger();

try
{
    Log.Information("Starting up");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

// Add services to the container.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(config.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));
builder.Services.AddDbContext<BookRecommendationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(BookRecommendationDbContext));
builder.Services.AddScoped<IRecurringJobManager, RecurringJobManager>();
builder.Services.AddScoped<IRecommendationCronJob, RecommendationCronJob>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 3;
    options.Queues = new[] { "recommendation_queue" };
});

//ProducerConfig producerConfig = config.GetSection("Producer").Get<ProducerConfig>();
//builder.Services.AddSingleton(producerConfig);
//ConsumerConfig consumerConfig = config.GetSection("Consumer").Get<ConsumerConfig>();
//builder.Services.AddSingleton(consumerConfig);

MapperConfiguration mappingConfig = new MapperConfiguration(x =>
{
    x.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseHangfireDashboard();

app.MapControllers();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync(" Cron Job is Up!");
});

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
HangFireSchedulerJob hangFireSchedulerJob = new HangFireSchedulerJob();
hangFireSchedulerJob.ScheduleRecurringJobs();

app.Run();
