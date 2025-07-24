using System.Collections.Generic;

namespace GameApi.Models
{
    public class MyDbContext
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Juego> Juegos { get; set; }
        }
    }
}
