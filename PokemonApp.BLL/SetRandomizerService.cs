
using PokemonApp.DAL;
namespace PokemonApp.BLL
{
    /// <summary>
    /// Interacts with our supabase database.
    /// Fetches a random set based on set criteria.
    /// </summary>
    public class SetRandomizerService
    {

        private Random _random;

        private SupabaseService _supabaseService;

        public SetRandomizerService(SupabaseService supabaseService)
        {
            _random = new Random();

            _supabaseService = supabaseService;

        }

        /// <summary>
        /// Fetches the setId of any random set.
        /// </summary>
        /// <returns>A task that resolves to a tuple containing the set ID and set name</returns>
        public async Task<(string SetId, string SetName)> RandomizeSetAsync()
        {
            Dictionary<string, string> SetAndTotal = await _supabaseService.FetchSetIdsAsync();


            int index = _random.Next(SetAndTotal.Count);

            var selectedSet = SetAndTotal.ElementAt(index);


            return (selectedSet.Key, selectedSet.Value);
        }

        /// <summary>
        /// Fetches a random set ID and name from the first 30 sets in the dataset,
        /// simulating a "throwback" set selection.
        /// </summary>
        /// <returns>A task that resolves to a tuple containing the set ID and set name.</returns>
        public async Task<(string SetId, string SetName)> RandomizeThrowbackSetAsync()
        {
            Dictionary<string, string> allSets = await _supabaseService.FetchSetIdsAsync();

            var throwBackSets = allSets.Take(30).ToList();

            var randomSet = throwBackSets[_random.Next(throwBackSets.Count)];

            return (randomSet.Key, randomSet.Value); 
        }
    }
}
