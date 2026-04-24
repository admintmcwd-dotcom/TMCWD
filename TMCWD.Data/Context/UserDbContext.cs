using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Context
{
    public class UserDbContext : DbContext
    {
        #region Fields

        public DbSet<Users> UserEntities { get; set; }

        #endregion

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;uid=sa;pwd=password123;database=tmcwd";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            base.OnConfiguring(optionsBuilder);
        }

        #endregion
    }
}
