using PokemonApp.DAL;
namespace PokemonApp.BLL
{
    public class PokemonCardFetcherService
    {
        /// <summary>
        /// Middleman between task and getting the card from the TCGAPI.
        /// Uses the SetRandomizerService to get a random SetID.
        /// Then said SetID is sent to the TCGService to aquire a card from the API.
        /// </summary>

        private SetRandomizerService _setRandomizerService;
        private PokemonTCGService _pokemonTCGService;

        public PokemonCardFetcherService(SetRandomizerService setRandomizerService, PokemonTCGService pokemonTCGService)
        {
            _setRandomizerService = setRandomizerService;
            _pokemonTCGService = pokemonTCGService;
        }

        /// <summary>
        /// Gets a high market price card from any of the available Pokémon sets.
        /// First get a random SetID from the stored sets.
        /// Sends SetID to TCGService to fetch a random top market price card.
        /// </summary>
        /// <returns></returns>
        public async Task<Card> AquireHighestMarketPriceCardAsync()
        {
            var (setId, setName) = await _setRandomizerService.RandomizeSetAsync();

            Card pokemonCard = await _pokemonTCGService.FetchRandomTopMarketPriceCardAsync(setId);

            pokemonCard.SetName = setName;

            return pokemonCard;
        }

        /// <summary>
        /// Same as previous method but acquires a "throw back" card. 
        /// This is a card from an older set with no focus on market price.
        /// </summary>
        /// <returns></returns>
        public async Task<Card> AquireThrowbackCardAsync()
        {
            var (setId, setName) = await _setRandomizerService.RandomizeThrowbackSetAsync();

            Card pokemonCard = await _pokemonTCGService.FetchRandomThrowbackCardAsync(setId);

            pokemonCard.SetName = setName;

            return pokemonCard;
        }
    }
}
