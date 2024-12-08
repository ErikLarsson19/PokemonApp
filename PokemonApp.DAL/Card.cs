using Supabase.Postgrest.Models;


namespace PokemonApp.DAL
{
    public class Card : BaseModel
    {

        public string Name { get; set; }
        public string Set { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }

        public string ImageUrl { get; set; }
        public decimal MarketPrice { get; set; }

        public string SetName { get; set; }
        public string PriceCategory { get; set; }

        public string ReleaseDate { get; set; }

        
        public Card(string name, decimal marketPrice, string imageUrl, string rarity, string priceCategory, string setName, string releaseDate)
        {
            Name = name;
            MarketPrice = marketPrice;
            ImageUrl = imageUrl;
            Rarity = rarity;
            PriceCategory = priceCategory;
            SetName = setName;
            ReleaseDate = releaseDate;
        }

    
        public Card()
        {

        }

        public override string ToString()
        {
            return $"Name: {Name}, Marketprice: {MarketPrice}, ImageUrl: {ImageUrl}";
        }
    }
}
