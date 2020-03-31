using System.Data.Entity;

using ShoppingWebApp.Models;

namespace ShoppingWebApp.DB
{
    public class ShoppingDbContext : DbContext
    {
        public ShoppingDbContext() : base("Server=localhost;Database=Team10bCA;Integrated Security=True;")
        {
            Database.SetInitializer(new ShoppingDbInitializer<ShoppingDbContext>());
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderDetailExtended> OrderDetailExtended { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}