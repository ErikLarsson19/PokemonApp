using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PokemonApp.BLL
{
    /// <summary>
    /// The ImageUtility class is used to help prepare images for upload to X and BlueSky.
    /// </summary>
    public static class ImageUtility
    {
        /// <summary>
        /// Downloads an image from a given URL and returns the raw byte array.
        /// </summary>
        /// <param name="imageUrl">Url of image to download</param>
        /// <returns>Raw byte array of image</returns>
        public static async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetByteArrayAsync(imageUrl);
        }

        /// <summary>
        /// Processes the image and prepares for upload to BlueSky.
        /// </summary>
        /// <param name="originalImageBytes">Raw byte array of original image</param>
        /// <returns>A task that resolves to a byte array representing the processed image</returns>
        public static Task<byte[]> ProcessImageForBlueSkyAsync(byte[] originalImageBytes)
        {
            return Task.Run(() =>
            {
                using var image = Image.Load(originalImageBytes);

                // Define the target aspect ratio (16:9 for BlueSky timeline previews)
                int targetWidth = 1200;
                int targetHeight = 675;

                // Calculate padding to maintain the original card image without cropping
                image.Mutate(x => x.Pad(targetWidth, targetHeight, Color.White));

                // Create a gradient effect over the padding
                using var gradientBackground = new Image<Rgba32>(targetWidth, targetHeight, Color.Transparent);
                gradientBackground.Mutate(ctx =>
                {
                    ctx.Fill(
                        new LinearGradientBrush(
                            new Point(0, 0),
                            new Point(targetWidth, targetHeight),
                            GradientRepetitionMode.None,
                            new ColorStop(0f, Color.LightGray),
                            new ColorStop(1f, Color.White)
                        )
                    );
                    ctx.DrawImage(image, new Point(0, 0), 1f);
                });

                using var memoryStream = new MemoryStream();

                // Save the padded image with compression
                image.Save(memoryStream, new PngEncoder
                {
                    CompressionLevel = PngCompressionLevel.BestCompression // Maximize compression to reduce file size
                });

                return memoryStream.ToArray();
            });
        }
    }
}
