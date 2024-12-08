using PokemonApp.BLL;
namespace PokemonApp
{
    public class Scheduler
    {
        private readonly ControlStation _controlStation;

        public Scheduler(ControlStation controlStation)
        {
            _controlStation = controlStation;
        }

        public async Task ScheduleTaskAsync(TimeSpan delay, string typeOfContent)
        {
            Console.WriteLine($"Task of type '{typeOfContent}' scheduled to run in {delay.TotalSeconds} seconds...");
            await Task.Delay(delay);

            // Trigger the task through the ControlStation
            await _controlStation.ProcessScheduledTask(typeOfContent);

            Console.WriteLine("Task execution completed.");
        }
    }
}
