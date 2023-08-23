using Microsoft.EntityFrameworkCore;
using asp_app.Models;

namespace asp_app.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Replace with your connection string.
            var connectionString = "server=localhost;user=root;password=password;database=asp_app";

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}