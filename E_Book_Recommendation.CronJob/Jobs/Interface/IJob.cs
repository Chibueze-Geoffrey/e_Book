namespace E_Book_Recommendation.CronJob.Jobs.Interface
{
    public interface IJob
    {
        Task RunAtTimeOf(DateTime now);
    }
}
