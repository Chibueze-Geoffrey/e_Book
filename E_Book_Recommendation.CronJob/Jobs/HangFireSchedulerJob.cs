using Hangfire;

namespace E_Book_Recommendation.CronJob.Jobs
{
    public class HangFireSchedulerJob
    {
        private readonly string _RecommendationJob = $"*/13 * * * *";


        public void ScheduleRecurringJobs()
        {
            RecurringJob.RemoveIfExists(nameof(RecommendationCronJob));
            RecurringJob.AddOrUpdate<RecommendationCronJob>(nameof(RecommendationCronJob),
                job => job.Run(JobCancellationToken.Null), _RecommendationJob);
        }

        }
    }
