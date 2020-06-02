
using System.Data.Entity;

//namespace NorthwindConsole.Models
{
namespace Nhoward_FinalProject.Models
    public class Nhoward_FinalProject : DbContext
    {
        public Nhoward_FinalProject() : base("name=NorthwindContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}