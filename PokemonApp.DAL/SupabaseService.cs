
using Supabase;


namespace PokemonApp.DAL
{
    /// <summary>
    /// Class to interact with our database containing all of the pokemon sets.
    /// </summary>
    public class SupabaseService
    {
        private readonly Client _client;

        private string supabasekey = Environment.GetEnvironmentVariable("SUPABASE_KEY");
        private readonly string supabaseurl = Environment.GetEnvironmentVariable("SUPABSE_URL");

        public SupabaseService()
        {
            _client = new Client(supabaseurl, supabasekey);
            _client.InitializeAsync().Wait();
        }

        /// <summary>
        /// Fetches the identifier (set id) and name of the set from the PokemonSet table.
        /// </summary>
        /// <returns>Dictionary containing the identifier and set name</returns>
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

