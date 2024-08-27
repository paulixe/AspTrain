using AspApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspApp.Data
{
    public class AspAppDbContext : IdentityDbContext<IdentityUser>
    {
        private IConfiguration configuration;
        public AspAppDbContext(IConfiguration config)
        {
            configuration = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<PostIt> Posts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Workshop> Workshops { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Workshop>().HasMany(w => w.ToolsAvailable).WithMany(t => t.AvailableAtWorkshops);

            modelBuilder.Entity<Tutorial>().HasMany(t => t.RequiredTools).WithMany(t => t.UsedInTutorials);

            modelBuilder.Entity<Booking>().HasMany(b => b.ItemsBooked).WithMany(t => t.Bookings);

            modelBuilder.Entity<PostIt>().HasData(
                new PostIt { Id = 1, Description = "Hello World 2 !!" },
                new PostIt { Id = 2, Description = "Coucou" }
                );

            modelBuilder.Entity<Tool>().HasData(
                new Tool { Id = 1, Name = "Knife" },
                new Tool { Id = 2, Name = "jigsaw" },
                new Tool { Id = 3, Name = "knitting machine" }
                );

            modelBuilder.Entity<Tutorial>().HasData(
                new Tutorial { Id = 1, Name = "Knitting tutorial" },
                new Tutorial { Id = 2, Name = "Carpentry tutorial" }
                );

            modelBuilder.Entity<Workshop>().HasData(
                new Workshop { Id = 1, Name = "Paix Dieu" },
                new Workshop { Id = 2, Name = "Villeurbanne" }
                );


        }
    }
}
