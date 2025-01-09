using E_Book_Recommendation.Business.Logic.Interface;
using E_Book_Recommendation.CronJob.Jobs.Interface;
using Hangfire;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Logging;

namespace E_Book_Recommendation.CronJob.Jobs
{
    [DisableConcurrentExecution(60 * 3600)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [Queue("recommendation_queue")]
    public class RecommendationCronJob: IRecommendationCronJob
    {
        private readonly IRecommendationService _recommendation;
        public ILogger<RecommendationCronJob> Logger { get; }
        public RecommendationCronJob(ILogger<RecommendationCronJob> logger, IRecommendationService recommendationService)
        {
            Logger= logger; 
            _recommendation=recommendationService;
        }
        public async Task RunAtTimeOf(DateTime now)
        {
            Logger.LogInformation("Running ProcessingrecommendationRetries Job @ {@date}", now);
            await _recommendation.GetRecommendationsAsync();
            Logger.LogInformation("ProcessingrecommendationRetries Job Completed @ {@date}", now);
        }
        public async Task Run(IJobCancellationToken token)
        {
            await RunAtTimeOf(DateTime.Now);
        }
    }

    public interface IRecommendationCronJob:IJob
    { }
}
