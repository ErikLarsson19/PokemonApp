
using Supabase;


namespace PokemonApp.DAL
{
    public class SupabaseService
    {
        private readonly Client _client;

        private string supabasekey = Environment.GetEnvironmentVariable("SUPABASE_KEY");

        

        public SupabaseService()
        {
            _client = new Client("https://fturujgsjfkhtqqwesla.supabase.co", supabasekey);
            _client.InitializeAsync().Wait();
        }


        public async Task<Dictionary<string, string>> FetchSetIdsAsync()
        {
            var identifiers = new Dictionary<string, string>();

            try
            {
                // Fetch only the 'identifier' and 'name' field from the 'PokemonSet' table
                var result = await _client
                    .From<PokemonSet>()
                    .Select("Identifier, Name")
                    .Get();

                // Iterate through the results and add each identifier to the list
                foreach (var item in result.Models)
                {
                    identifiers.Add(item.Identifier, item.Name);
                }

                Console.WriteLine("Identifiers fetched successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to fetch identifiers: {e.Message}");
            }

            return identifiers;
        }

    }
}

