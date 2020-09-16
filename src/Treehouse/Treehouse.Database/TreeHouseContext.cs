using Microsoft.EntityFrameworkCore;
using TreeHouse.Database.Models;

namespace TreeHouse.Database
{
    //https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=visual-studio
    public class TreeHouseContext : DbContext
    {
        private readonly string _dataSource;
        public TreeHouseContext()
        {
            _dataSource = @"TreeHouse.sqlite";
        }

        public TreeHouseContext(string dataSource)
        {
            _dataSource = dataSource;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobComment> JobComments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dataSource}");
    }
}
