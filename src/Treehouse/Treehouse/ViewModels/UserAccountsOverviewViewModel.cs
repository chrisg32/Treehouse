using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeHouse.Services;

namespace TreeHouse.ViewModels
{
    public class UserAccountsOverviewViewModel : ViewModelBase
    {
        public List<AccountViewModel> Accounts { get; private set; }
        public UserAccountsOverviewViewModel(DbService dbService) : base(dbService)
        {
        }

        public async Task LoadAsync(int userId, bool isCash)
        {
            await using var connection = CreateConnection();
            var query = await connection.Accounts.Where(a => a.Cash == isCash && a.UserId == userId).ToListAsync();
            var user = await connection.Users.FirstAsync(u => u.Id == userId);

            var accounts = query.Select(a => new AccountViewModel(_dbService)
            {
                Id = a.Id,
                Name = a.Name,
                User = user
            })
                .OrderByDescending(a => a.Name == "Long Term Savings")
                .ThenByDescending(a => a.Name == "Tithe")
                .ThenBy(a => a.Name)
                .ToList();

            foreach (var account in accounts)
            {
                var accountId = account.Id;
                account.Transactions = await connection.Transactions.Where(t => t.AccountId == accountId).OrderByDescending(t => t.Timestamp).ToListAsync();
            }

            Accounts = accounts;
        }
    }
}
