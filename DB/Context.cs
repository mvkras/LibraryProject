using Diplom.Krasnov__WindowsForms.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Diplom.Krasnov__WindowsForms.DB
{
    public class EFContext : DbContext
    {
        public EFContext(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<TakenBook> TakenBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
