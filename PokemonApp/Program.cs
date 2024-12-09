using PokemonApp;
using PokemonApp.BLL;
using PokemonApp.BLL.Tasks;
using PokemonApp.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
class Program
{
    //Double check the code
    //Add environment to local dev
    //Comments
    //Test scheduler during day 1 hour delay then swap to 6 for deployment.
    //Check posts made and correct formatting so they look better.
    //Old posts look weird on bluesky figure out a way to fix that. 
    

    //Size too big issue styll
    //Fix error messages or see what can be done about those. 
    //Test deployment again with twitter.
    static async Task Main(string[] args)
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddSingleton<SupabaseService>(); // Singleton as it maintains one client instance
        builder.Services.AddTransient<SetRandomizerService>(); // Transient for lightweight stateless logic
        builder.Services.AddSingleton<PokemonTCGService>(); // Singleton as it performs API interactions
        builder.Services.AddTransient<PokemonCardFetcherService>(); // Transient for card fetching logic
        builder.Services.AddSingleton<TwitterService>(); // Singleton as external communication is stateless
        builder.Services.AddSingleton<BlueskyPostService>(); // Singleton for posting logic
        builder.Services.AddSingleton<Scheduler>(); // Singleton for managing scheduled tasks
        builder.Services.AddTransient<HighMarketPriceTaskHandler>(); // Transient as tasks are stateless
        builder.Services.AddTransient<ThrowbackCardTaskHandler>(); // Transient as tasks are stateless
        builder.Services.AddSingleton<ControlStation>(provider =>
        {
            var highMarketPriceTaskHandler = provider.GetRequiredService<HighMarketPriceTaskHandler>();
            var throwbackCardTaskHandler = provider.GetRequiredService<ThrowbackCardTaskHandler>();

            // Task handlers mapped by type
            var taskHandlers = new Dictionary<string, IScheduledTaskHandler>
            {
                {"highMarketPrice", highMarketPriceTaskHandler},
                {"throwbackCard", throwbackCardTaskHandler}
            };

            return new ControlStation(taskHandlers);
        });
        builder.Services.AddHostedService<PostSchedulerService>(); // Hosted service for scheduling posts

        var app = builder.Build();

        Console.WriteLine("Starting Post Scheduler...");
        await app.RunAsync(); // Starts the hosted service
    }

}
