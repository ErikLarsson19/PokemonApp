using Supabase;
using Supabase.Postgrest.Models;

namespace PokemonApp.DAL
{
    public class SupabaseAddSet
    {
        private readonly Client _client;

        private string _supabasekey = Environment.GetEnvironmentVariable("SUPABASE_APIKEY");
        public SupabaseAddSet()
        {
            Console.WriteLine("Entering supabase constructor");
            _client = new Client("https://fturujgsjfkhtqqwesla.supabase.co", _supabasekey);
            _client.InitializeAsync().Wait();
        }

        public async Task InserCardsAsync<T>(List<T> objects) where T : BaseModel, new()
        {
            Console.WriteLine("Entering insert cards async supabase");
            Console.WriteLine($"Attempting to insert {objects.Count} objects.");

            foreach (var obj in objects)
            {
                Console.WriteLine(obj.ToString());
            }

            try
            {
                await _client.From<T>().Insert(objects);
                Console.WriteLine("Objects successfully entered database");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error inserting objects: {e.Message}");
                Console.WriteLine($"Stack Trace: {e.StackTrace}");
                throw;
            }
        }


    }
}
