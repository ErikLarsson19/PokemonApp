
namespace PokemonApp.BLL.Tasks
{
    /// <summary>
    /// See HighMarketPriceTaskHandler for more information.
    /// The only difference from this handler and the one above is that 
    /// this handler fires of "ThrowBack" cards. Cards that are older and 
    /// without a focus on market price.
    /// </summary>
    public class ThrowbackCardTaskHandler : IScheduledTaskHandler
    {
        private readonly PokemonCardFetcherService _pokemonCardFetcherService;
        private readonly TwitterService _twitterService;
        private readonly BlueskyPostService _blueskyPostService;
        private readonly PostContentHandler _postContentHandler;


        public ThrowbackCardTaskHandler(PokemonCardFetcherService pokemonCardFetcherService, TwitterService twitterService, BlueskyPostService blueskyPostService)
        {
            _pokemonCardFetcherService = pokemonCardFetcherService;
            _twitterService = twitterService;
            _blueskyPostService = blueskyPostService;
            _postContentHandler = new PostContentHandler();
        }
       

        public async Task HandleTaskAsync()
        {
            var card = await _pokemonCardFetcherService.AquireThrowbackCardAsync();

            string postContent = _postContentHandler.GenerateThrowbackPostContent(card);

            //await _twitterService.PostTweetAsync(postContent, card.ImageUrl);
            await _blueskyPostService.CreatePostWithImageAsync(postContent, card.ImageUrl);

        }

        
    }
}
