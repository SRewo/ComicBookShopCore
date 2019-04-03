using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Data
{
    public class ShopDbEntities : DbContext
    {

        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<ComicBook> ComicBooks { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ComicBookArtist> ComicBookArtists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source = localhost; Initial Catalog = ComicBookShopCore; Integrated Security = true");
        }
    }
}
