using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GameApi.Models
{
        public class MyDbContext : DbContext
        {
            public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

            public DbSet<Game> Games { get; set; }

        //cambio de parametros de precision de decimal para Precio
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(10, 2); 

        }

    }


}
