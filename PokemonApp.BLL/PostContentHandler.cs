
using PokemonApp.DAL;
namespace PokemonApp.BLL
{
    /// <summary>
    /// Handles the generation of post content for different types of card posts.
    /// </summary>
    public class PostContentHandler
    {
        private static readonly Random _random = new();

        private readonly List<string> _throwbackPostTemplates = new()
        {
            "Throwback to {0}!\nFrom the {1} set. 🎉\nReleased in {2}.\nDo you still have this one? #PokemonTCG",
            "Who remembers {0}?\nFrom the {1} set,\nReleased in {2}. 🌟\nSuch a classic! #PokemonCollectibles",
            "{0} from {1}!\nReleased in {2}. ✨\nA timeless gem. #Pokemon",
            "Today's throwback:\n{0} from the {1} set! 📜\nReleased in {2}.\nA true collector's dream! #PokemonTCG",
            "Revisiting {0}.\nFrom the {1} set. 💭\nReleased in {2}.\nGot any cards from this set? #Pokemon",
            "{0} from {1}!\nReleased in {2}. 🚀\nA trip down memory lane! #PokemonTCG",
        };


        private readonly List<string> _highMarketPostTemplates = new()
        {
            "Check this out:\n{0} from {1}! 💰\nTrending at ${2}.\n#PokemonTCG",
            "Big mover alert:\n{0} from {1}! 💎\nValued at ${2}.\nAnyone have this? #Pokemon",
            "Market spotlight:\n{0} ({1}).\nWorth ${2}. 🎴\nA true collector’s prize! #PokemonCollecting",
            "{0} from {1}!\nNow worth ${2}. 💰\nIs this in your collection? #PokemonTCG",
            "{0} from {1}.\nTrending at ${2}. 🏆\n#Pokemon",
            "This gem:\n{0} from {1}.\nValued at ${2}. 💎\nIconic! #PokemonCollecting",
        };



        /// <summary>
        /// Generates post content for a throwback card.
        /// </summary>
        public string GenerateThrowbackPostContent(Card card)
        {
            // Randomly pick a template
            string template = _throwbackPostTemplates[_random.Next(_throwbackPostTemplates.Count)];
            return string.Format(template, card.Name, card.SetName, card.ReleaseDate);
        }

        /// <summary>
        /// Generates post content for a high-market-price card.
        /// </summary>
        public string GenerateHighMarketPricePostContent(Card card)
        {
            // Randomly pick a template
            string template = _highMarketPostTemplates[_random.Next(_highMarketPostTemplates.Count)];
            return string.Format(template, card.Name, card.SetName, card.MarketPrice);
        }
    }
}
