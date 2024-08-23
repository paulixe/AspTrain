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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostIt>().HasData(
                new PostIt { Id = 1, Description = "Hello World 2 !!" }

                );
        }
    }
}
