using System;
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

            var accounts = query.Select(a => new AccountViewModel
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();

            foreach (var account in accounts)
            {
                var accountId = account.Id;
                account.Balance = (await connection.Transactions.Where(t => t.AccountId == accountId).ToListAsync()).Sum(t => t.Amount);
            }

            Accounts = accounts;
        }
    }
}
