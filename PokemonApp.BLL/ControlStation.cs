using PokemonApp.BLL.Tasks;

namespace PokemonApp.BLL
{
    public class ControlStation
    {

        private readonly Dictionary<string, IScheduledTaskHandler> _taskHandlers;
        public ControlStation(Dictionary<string, IScheduledTaskHandler> taskHandlers)
        {
            _taskHandlers = taskHandlers;
        }


        public async Task ProcessScheduledTask(string typeOfContent)
        {
            if (_taskHandlers.TryGetValue(typeOfContent, out var handler))
            {
                await handler.HandleTaskAsync();
            }
            else
            {
                Console.WriteLine($"No handler found for content type {typeOfContent}");
            }
        }




    }
}
