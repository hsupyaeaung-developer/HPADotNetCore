using HPADotNetCore.MinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HPADotNetCore.MinimalApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogDataModel> Blogs { get; set; }
    }
}
