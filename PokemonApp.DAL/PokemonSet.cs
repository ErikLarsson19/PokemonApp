using Supabase.Postgrest.Models;

namespace PokemonApp.DAL
{
    public class PokemonSet : BaseModel
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public int Total { get; set; }

        public string ReleaseDate { get; set; }

        public string LogoUrl { get; set; }

        public override string ToString()
        {
            return $"Identifier: {Identifier}, Name: {Name}, Total: {Total}, ReleaseDate: {ReleaseDate}, LogoUrl: {LogoUrl}";
        }


    }
}
