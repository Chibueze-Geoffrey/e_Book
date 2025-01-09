using E_Book_Recommendation.Business.Logic.Implementation;
using E_Book_Recommendation.Business.Logic.Interface;
using E_Book_Recommendation.Common.Mapping;
using E_Book_Recommendation.Data.DataAccess;
using E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces;
using E_Book_Recommendation.Data.Repository;
using E_Book_Recommendation.Data.UnitOfWork_;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<BookRecommendationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
