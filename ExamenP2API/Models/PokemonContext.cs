using System.Data.Entity;

namespace ExamenP2API.Models
{
    public class PokemonContext : DbContext
    {
        public PokemonContext() : base("name=PokemonConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PokemonContext>());
        }

        public DbSet<PokemonModel> PokemonModels { get; set; }
    }
}