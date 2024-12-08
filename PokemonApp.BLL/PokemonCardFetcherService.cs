using PokemonApp.DAL;
namespace PokemonApp.BLL
{
    public class PokemonCardFetcherService
    {

        private SetRandomizerService _setRandomizerService;
        private PokemonTCGService _pokemonTCGService;

        public PokemonCardFetcherService(SetRandomizerService setRandomizerService, PokemonTCGService pokemonTCGService)
        {
            _setRandomizerService = setRandomizerService;
            _pokemonTCGService = pokemonTCGService;
        }

        public async Task<Card> AquireHighestMarketPriceCardAsync()
        {
            var (setId, setName) = await _setRandomizerService.RandomizeSetAsync();

            Card pokemonCard = await _pokemonTCGService.FetchRandomTopMarketPriceCardAsync(setId);

            pokemonCard.SetName = setName;

            return pokemonCard;
        }

        public async Task<Card> AquireThrowbackCardAsync()
        {
            var (setId, setName) = await _setRandomizerService.RandomizeThrowbackSetAsync();

            Card pokemonCard = await _pokemonTCGService.FetchRandomThrowbackCardAsync(setId);

            pokemonCard.SetName = setName;

            return pokemonCard;
        }
    }
}
