
using System.Text.Json;


namespace PokemonApp.DAL
{
    public class PokemonTCGService
    {

        private readonly HttpClient _httpClient;
        private string _tcgApiKey = Environment.GetEnvironmentVariable("TCG_APIKEY");

        public PokemonTCGService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _tcgApiKey);
        }


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
