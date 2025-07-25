using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GameApi.Models
{
        public class MyDbContext : DbContext
        {
            public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

            public DbSet<Game> Games { get; set; }

        }
}
