
namespace PokemonApp.BLL.Tasks
{
    /// <summary>
    /// The task handler for sending out a high market price Pokémon card to twitter and bluesky.
    /// </summary>
    public class HighMarketPriceTaskHandler : IScheduledTaskHandler
    {

        private readonly PokemonCardFetcherService _cardFetchService;
        private readonly TwitterService _twitterService;
        private readonly BlueskyPostService _blueskyPostService;
        private readonly PostContentHandler _postContentHandler;

        public HighMarketPriceTaskHandler(PokemonCardFetcherService pokemonCardFetcherService, TwitterService twitterService, BlueskyPostService blueskyPostService)
        {
            _cardFetchService = pokemonCardFetcherService;
            _twitterService = twitterService;
            _blueskyPostService = blueskyPostService;
            _postContentHandler = new PostContentHandler();
        }

        /// <summary>
        /// Fetches the card from PokemonCardFetcherService.
        /// Then gets a random message from PostContentHandler and saves it as a string.
        /// Fires the post created with content in the form of a string and a Card object to Twitter (X) and BlueSky.
        /// </summary>
        /// <returns></returns>
        public async Task HandleTaskAsync()
        {
            var card = await _cardFetchService.AquireHighestMarketPriceCardAsync();

            string tweetContent = _postContentHandler.GenerateHighMarketPricePostContent(card);

            await _twitterService.PostTweetAsync(tweetContent, card.ImageUrl);
            await _blueskyPostService.CreatePostWithImageAsync(tweetContent, card.ImageUrl);
        }
    }
}
