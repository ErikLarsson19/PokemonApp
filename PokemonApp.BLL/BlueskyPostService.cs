using FishyFlip;
using FishyFlip.Models;
using System.Net.Http.Headers;
using X.Bluesky.Models;


namespace PokemonApp.BLL
{
    public class BlueskyPostService
    {

        private string identifier = Environment.GetEnvironmentVariable("BLUESKY_IDENTIFIER");
        private string password = Environment.GetEnvironmentVariable("BLUESKY_PASSWORD");

        private readonly ATProtocol _atProtocol;

        public BlueskyPostService()
        {
            _atProtocol = InitializeProtocol();
        }

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