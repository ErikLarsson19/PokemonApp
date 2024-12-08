using PokemonApp.DAL;

namespace PokemonApp.BLL.Tasks

{
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

        public async Task HandleTaskAsync()
        {
            var card = await _cardFetchService.AquireHighestMarketPriceCardAsync();

            string tweetContent = _postContentHandler.GenerateHighMarketPricePostContent(card);

            //await _twitterService.PostTweetAsync(tweetContent, card.ImageUrl);
            await _blueskyPostService.CreatePostWithImageAsync(tweetContent, card.ImageUrl);
        }


       

    }
}
