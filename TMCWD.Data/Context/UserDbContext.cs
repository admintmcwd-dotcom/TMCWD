using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Context
{
    public class UserDbContext : DbContext
    {
        #region Fields

        string connectionString = "server=localhost;port=3306;database=tmcwd;user=root;password=password123;";

        public DbSet<User> Users { get; set; }

        public DbSet<InspectionType> InspectionTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<InspectionTypeDetail> InspectonTypeDetails { get; set; }

        #endregion

        #region constructors

        public UserDbContext(): base()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        #endregion

        #region methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            base.OnConfiguring(optionsBuilder);
        }

        #endregion
    }
}
