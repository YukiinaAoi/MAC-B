// Data/QRAppDbContext.cs
using Microsoft.EntityFrameworkCore;
using QRAppAPI.Models;

namespace QRAppAPI.Data
{
    public class QRAppDbContext : DbContext
    {
        public QRAppDbContext(DbContextOptions<QRAppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
