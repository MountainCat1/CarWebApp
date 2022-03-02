using CarWebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWebApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}