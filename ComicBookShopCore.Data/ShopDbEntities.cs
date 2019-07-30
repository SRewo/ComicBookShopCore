using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace ComicBookShopCore.Data
{
    public class ShopDbEntities : IdentityDbContext<User>
    {
        public ShopDbEntities() : base()
        {
            AddRoles();
        }

        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<ComicBook> ComicBooks { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> Employees { get; set; }
        public virtual DbSet<ComicBookArtist> ComicBookArtists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source = localhost; Initial Catalog = ComicBookShopCore; Integrated Security = true");
        }

        private async Task AddRoles()
        {
            var roleStore = new RoleStore<IdentityRole>(this);

            if (roleStore.Roles.Any()) return;

            var roleUser = new IdentityRole("User") {NormalizedName = "USER"};
            await roleStore.CreateAsync(roleUser);
            var roleEmployee = new IdentityRole("Employee") {NormalizedName = "EMPLOYEE"};
            await roleStore.CreateAsync(roleEmployee);
            var roleAdmin = new IdentityRole("Admin") {NormalizedName = "ADMIN"};
            await roleStore.CreateAsync(roleAdmin);
            var roleOwner = new IdentityRole("Owner") {NormalizedName = "OWNER"};
            await roleStore.CreateAsync(roleOwner);
        }
    }
}