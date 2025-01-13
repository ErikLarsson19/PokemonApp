using PokemonApp.BLL;
namespace PokemonApp
{
    /// <summary>
    /// Scheduler is responsible for scheduling tasks with a specified delay
    /// and then delegating their exection to the ControlStation.
    /// </summary>
    public class Scheduler
    {
        private readonly ControlStation _controlStation;

        public Scheduler(ControlStation controlStation)
        {
            _controlStation = controlStation;
        }

        /// <summary>
        /// Schedules a task to be exectued after specified delay
        /// </summary>
        /// <param name="delay">The time span before executing the task</param>
        /// <param name="typeOfContent">What type of post we will be making</param>
        /// <returns>Task responsible for the async operation</returns>
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
