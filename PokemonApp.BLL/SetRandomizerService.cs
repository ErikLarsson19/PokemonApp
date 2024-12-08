
using PokemonApp.DAL;
namespace PokemonApp.BLL
{
    public class SetRandomizerService
    {

        private Random _random;

        private SupabaseService _supabaseService;

        public SetRandomizerService(SupabaseService supabaseService)
        {
            _random = new Random();

            _supabaseService = supabaseService;

        }

        public async Task<(string SetId, string SetName)> RandomizeSetAsync()
        {
            Dictionary<string, string> SetAndTotal = await _supabaseService.FetchSetIdsAsync();


            int index = _random.Next(SetAndTotal.Count);

            var selectedSet = SetAndTotal.ElementAt(index);


            return (selectedSet.Key, selectedSet.Value);
        }

        public async Task<(string SetId, string SetName)> RandomizeThrowbackSetAsync()
        {
            Dictionary<string, string> allSets = await _supabaseService.FetchSetIdsAsync();

            var throwBackSets = allSets.Take(30).ToList();

            var randomSet = throwBackSets[_random.Next(throwBackSets.Count)];

            return (randomSet.Key, randomSet.Value); 
        }
    }
}
