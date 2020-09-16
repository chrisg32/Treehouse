using Microsoft.EntityFrameworkCore;
using TreeHouse.Database.Models;

namespace TreeHouse.Database
{
    //https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=visual-studio
    public class TreeHouseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=TreeHouse.sqlite");
    }
}
