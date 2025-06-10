using Microsoft.EntityFrameworkCore;
using WebApplicationDemo.Model;

namespace WebApplicationDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            // 保证数据库和表自动创建
            Database.EnsureCreated();
        }

        public DbSet<User> Users => Set<User>();
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
