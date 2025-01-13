using PokemonApp.BLL.Tasks;

namespace PokemonApp.BLL
{
    /// <summary>
    /// Centralized manager for executing tasks based on their type of content specified.
    /// It then delegates them to the appropriate task handlers based on the type of task.
    /// </summary>
    public class ControlStation
    {

        private readonly Dictionary<string, IScheduledTaskHandler> _taskHandlers;
        public ControlStation(Dictionary<string, IScheduledTaskHandler> taskHandlers)
        {
            _taskHandlers = taskHandlers;
        }

        /// <summary>
        /// Process the scheduled task by delegating it to the appropriate handler.
        /// </summary>
        /// <param name="typeOfContent">The type of task to process.</param>
        /// <returns></returns>
        public async Task ProcessScheduledTask(string typeOfContent)
        {
            if (_taskHandlers.TryGetValue(typeOfContent, out var handler))
            {
                await handler.HandleTaskAsync();
            }
            else
            {
                //Logs message if no handler is found with given task type.
                Console.WriteLine($"No handler found for content type {typeOfContent}");
            }
        }




    }
}
