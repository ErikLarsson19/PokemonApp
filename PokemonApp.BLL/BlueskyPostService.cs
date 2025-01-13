using FishyFlip;
using FishyFlip.Models;
using System.Net.Http.Headers;
using X.Bluesky.Models;


namespace PokemonApp.BLL
{
    /// <summary>
    /// This class handles sending the created post to BlueSky.
    /// It uses the FishyFlip package.
    /// </summary>
    public class BlueskyPostService
    {

        private string identifier = Environment.GetEnvironmentVariable("BLUESKY_IDENTIFIER");
        private string password = Environment.GetEnvironmentVariable("BLUESKY_PASSWORD");

        private readonly ATProtocol _atProtocol;

        public BlueskyPostService()
        {
            _atProtocol = InitializeProtocol();
        }

        /// <summary>
        /// Builds the protocol necessary to post on BlueSky.
        /// Authenticates using our identifier and password.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private ATProtocol InitializeProtocol()
        {
            var protocolBuilder = new ATProtocolBuilder()
                .EnableAutoRenewSession(true)
                .Build();

            var session = protocolBuilder.AuthenticateWithPasswordAsync(identifier, password).GetAwaiter().GetResult();

            if (session is null)
            {
                throw new Exception("Failed to authenticate with BlueSky");
            }

            return protocolBuilder;
        }

        /// <summary>
        /// Creating a post with an image on BlueSky can be a bit tricky. 
        /// In this method we use the ImageUtility class to download and clean the image so its ready to be posted.
        /// Then the method fires the post created to BlueSky.
        /// </summary>
        /// <param name="content">This is the text displayed inside the post</param>
        /// <param name="imageUrl">An image url of the selected Pokémon card</param>
        /// <returns></returns>
        public async Task CreatePostWithImageAsync(string content, string imageUrl)
        {
            try
            {
                // Download the image from the URL
                byte[] imageBytes = await ImageUtility.DownloadImageAsync(imageUrl);

                byte[] processedImageBytes = await ImageUtility.ProcessImageForBlueSkyAsync(imageBytes);

                using var stream = new MemoryStream(processedImageBytes);
                var imageContent = new StreamContent(stream)
                {
                    Headers =
            {
                ContentLength = stream.Length,
                ContentType = new MediaTypeHeaderValue("image/png") // Adjust MIME type as needed
            }
                };

                var blobResult = await _atProtocol.Repo.UploadBlobAsync(imageContent);

                await blobResult.SwitchAsync(
                    async success =>
                    {
                        var image = success.Blob.ToImage();

                        var postResult = await _atProtocol.Repo.CreatePostAsync(
                            content,
                            embed: new ImagesEmbed(image, "Pokémon!"));

                        postResult.Switch(
                            postSuccess =>
                            {
                                Console.WriteLine($"BlueSky Post with Image Created: {postSuccess.Uri} {postSuccess.Cid}");
                            },
                            postError =>
                            {
                                Console.WriteLine($"Error Creating BlueSky Post with Image: {postError.StatusCode} {postError.Detail}");
                            });
                    },
                    async error =>
                    {
                        Console.WriteLine($"Error Uploading Blob: {error.StatusCode} {error.Detail}");
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error posting to BlueSky: {ex.Message}");
            }
        }
    }
}