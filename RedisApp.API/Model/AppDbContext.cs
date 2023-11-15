using Microsoft.EntityFrameworkCore;

namespace RedisApp.API.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Kalem1", Price = 100 },
                new Product() { Id = 2, Name = "Kalem2", Price = 200 },
                new Product() { Id = 3, Name = "Kalem3", Price = 300 }
                );

            base.OnModelCreating(modelBuilder);
        }

    }
}
