using Microsoft.Extensions.Hosting;

namespace PokemonApp
{
    public class PostSchedulerService : BackgroundService
    {
        private readonly Scheduler _scheduler;

        // Constructor Dependency Injection
        public PostSchedulerService(Scheduler scheduler)
        {
            _scheduler = scheduler;
        }

        // Entry Point for Background Tasks
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Run the first post immediately
            await _scheduler.ScheduleTaskAsync(TimeSpan.Zero, "throwbackCard");

            while (!stoppingToken.IsCancellationRequested) // Check if the service should keep running
            {
                // Wait 10 hours and post "highMarketPrice"
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
                await _scheduler.ScheduleTaskAsync(TimeSpan.Zero, "highMarketPrice");

                // Wait another 10 hours and post "throwbackCard"
                await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
                await _scheduler.ScheduleTaskAsync(TimeSpan.Zero, "throwbackCard");
            }
        }
    }
}
