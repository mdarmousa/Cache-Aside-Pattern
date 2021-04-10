using Microsoft.EntityFrameworkCore;

namespace Cache_Aside_Pattern.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<Employee> Employees { get; set; }
    }
}
