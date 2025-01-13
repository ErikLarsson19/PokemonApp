using System.Text.Json;

namespace PokemonApp.DAL
{
    /// <summary>
    /// Responsible for fetching card details from the PokemonTCG Api.
    /// </summary>
    public class PokemonTCGService
    {

        private readonly HttpClient _httpClient;
        private string _tcgApiKey = Environment.GetEnvironmentVariable("TCG_APIKEY");

        public PokemonTCGService()
        {
            //Init HTTPClient and set the API key in the request header.
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _tcgApiKey);
        }

        /// <summary>
        /// Fetches a random card from the top precentile (top 15%) of the given set.
        /// </summary>
        /// <param name="setId">Id of the specified set</param>
        /// <param name="percentile">Top 15% of cards market price</param>
        /// <returns>A random card from the top precentile</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Card> FetchRandomTopMarketPriceCardAsync(string setId, double percentile = 0.15)
        {
            string url = $"https://api.pokemontcg.io/v2/cards?q=set.id:{setId}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(responseData);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.GetArrayLength() > 0)
            {
                var cards = dataElement.EnumerateArray()
                    .Select(card => ExtractCardDetails(card))
                    .Where(card => card.MarketPrice > 0) // Only include cards with valid market prices
                    .OrderByDescending(card => card.MarketPrice) // Sort by price descending
                    .ToList();

                if (cards.Count == 0)
                {
                    throw new Exception($"No cards with valid market prices found for set ID: {setId}");
                }

                int topPercentileCount = (int)Math.Ceiling(cards.Count * percentile);
                if (topPercentileCount <= 0)
                {
                    throw new Exception($"Calculated top percentile count is invalid for set ID: {setId}");
                }

                var random = new Random();
                var selectedCard = cards[random.Next(0, Math.Min(topPercentileCount, cards.Count))];

                return selectedCard;
            }

            throw new Exception($"No cards found for set ID: {setId}");
        }

        /// <summary>
        /// Fetches a random "throw back" card from the given set id.
        /// </summary>
        /// <param name="setId">The targeted set</param>
        /// <returns>A card from an older set</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Card> FetchRandomThrowbackCardAsync(string setId)
        {
            string url = $"https://api.pokemontcg.io/v2/cards?q=set.id:{setId}";


            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(responseData);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("data", out JsonElement dataElement) && dataElement.GetArrayLength() > 0)
            {
                var cards = dataElement.EnumerateArray()
                    .Select(card => ExtractCardDetails(card))
                    .ToList();

                var random = new Random();
                var randomCard = cards[random.Next(cards.Count)];

                return randomCard;
            }
            throw new Exception($"No cards found for set ID: {setId}");
        }

        /// <summary>
        /// Extracts the details of a card from a JSON element
        /// </summary>
        /// <param name="card">JsonElement representing a card</param>
        /// <returns>A card object with extracted details</returns>
        private Card ExtractCardDetails(JsonElement card)
        {
            // Basic details
            string name = card.GetProperty("name").GetString() ?? "Unknown";
            string setName = card.GetProperty("set").GetProperty("name").GetString() ?? "Unknown";
            string rarity = card.TryGetProperty("rarity", out JsonElement rarityElement) ? rarityElement.GetString() ?? "Unknown" : "Unknown";
            string imageUrl = card.GetProperty("images").GetProperty("large").GetString() ?? "No image available";

            // Extract market prices for different categories
            decimal? holofoilMarketPrice = ExtractPrice(card, "holofoil", "market");
            decimal? reverseHolofoilMarketPrice = ExtractPrice(card, "reverseHolofoil", "market");
            decimal? normalMarketPrice = ExtractPrice(card, "normal", "market");
            string releaseDate = card.GetProperty("set").GetProperty("releaseDate").GetString() ?? "1970-01-01";
            string releaseYear = DateTime.Parse(releaseDate).Year.ToString();
            // Determine the highest market price and its category
            var prices = new Dictionary<string, decimal?>
            {
                { "Holofoil", holofoilMarketPrice },
                { "Reverse Holofoil", reverseHolofoilMarketPrice },
                { "Normal", normalMarketPrice }
            };

            var bestPrice = prices.Where(p => p.Value.HasValue)
                                  .OrderByDescending(p => p.Value)
                                  .FirstOrDefault();

            return new Card(
                name: name,
                marketPrice: bestPrice.Value ?? 0,
                imageUrl: imageUrl,
                rarity: rarity,
                priceCategory: bestPrice.Key,
                setName: setName,
                releaseDate: releaseYear
            );
        }

        /// <summary>
        /// Used to extract the price from the cards data
        /// </summary>
        /// <param name="card">Json element representing the card</param>
        /// <param name="format">Format of the card (e.g "holofoil"</param>
        /// <param name="priceType">The type of price (e.g "market")</param>
        /// <returns>Returns the price if found.</returns>
        private decimal? ExtractPrice(JsonElement card, string format, string priceType)
        {
            if (card.TryGetProperty("tcgplayer", out JsonElement tcgplayerElement) &&
                tcgplayerElement.TryGetProperty("prices", out JsonElement pricesElement) &&
                pricesElement.TryGetProperty(format, out JsonElement formatElement) &&
                formatElement.TryGetProperty(priceType, out JsonElement priceElement) &&
                priceElement.ValueKind == JsonValueKind.Number)
            {
                return priceElement.GetDecimal();
            }

            return null; // Return null if no valid price is found
        }

        /// <summary>
        /// Fetches all the sets from the Pokémon TCG Api
        /// needs update.
        /// </summary>
        /// <returns>Returns the string of all the sets.</returns>
        public async Task<string> FetchAllTheSetsAsync()
        {

            string url = $"https://api.pokemontcg.io/v2/sets";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseData = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Card data:");
            //Console.WriteLine(responseData);

            return responseData;
        }

       
    }
}
