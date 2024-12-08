using LinqToTwitter.OAuth;
using LinqToTwitter;

namespace PokemonApp.BLL
{
    public class TwitterService
    {

      
        private string _apikey = Environment.GetEnvironmentVariable("TWITTER_APIKEY");
        private string _apikeysecret = Environment.GetEnvironmentVariable("TWITTER_APIKEYSECRET");
        private string _accesstoken = Environment.GetEnvironmentVariable("TWITTER_ACCESSTOKEN");
        private string _accesstokensecret = Environment.GetEnvironmentVariable("TWITTER_ACCESSTOKENSECRET");


        public TwitterService()
        {
        }

        public async Task PostTweetAsync(string tweetText, string imageUrl)
        {
            // Initialize Twitter context
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = _apikey,
                    ConsumerSecret = _apikeysecret,
                    AccessToken = _accesstoken,
                    AccessTokenSecret = _accesstokensecret
                }
            };

            var twitterCtx = new TwitterContext(auth);

            try
            {
                // Download image as byte array
                byte[] imageBytes = await ImageUtility.DownloadImageAsync(imageUrl);


                // Upload the image to Twitter
                var media = await twitterCtx.UploadMediaAsync(
                    imageBytes,
                    "image/jpeg",    // MIME type for the image
                    "tweet_image"); // Media category for a tweet image

                if (media == null || media.MediaID == 0)
                {
                    Console.WriteLine("Image upload failed.");
                    return;
                }
                var mediaIds = new List<string> { media.MediaID.ToString() };


                // Post the tweet with the uploaded image
                var tweet = await twitterCtx.TweetMediaAsync(tweetText, mediaIds);

                if (tweet != null)
                {
                    Console.WriteLine($"Tweet posted successfully with image: {tweet.Text}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error posting tweet with image: {ex.Message}");
            }
        }

        

      
    }
}