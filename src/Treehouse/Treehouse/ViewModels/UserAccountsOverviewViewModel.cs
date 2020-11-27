using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeHouse.Database.Models;
using TreeHouse.Services;

namespace TreeHouse.ViewModels
{
    public class UserAccountsOverviewViewModel : ViewModelBase
    {
        private readonly TokenAuthenticationStateProvider _tokenAuthenticationStateProvider;
        public List<AccountViewModel> Accounts { get; private set; }
        public UserAccountsOverviewViewModel(DbService dbService, TokenAuthenticationStateProvider tokenAuthenticationStateProvider) : base(dbService)
        {
            _tokenAuthenticationStateProvider = tokenAuthenticationStateProvider;
        }

        public async Task LoadAsync(bool isCash)
        {
            var state = await _tokenAuthenticationStateProvider.GetAuthenticationStateAsync();

            int userId = 0;

            if (state.User.HasClaim(c => c.Type == Claims.UserId))
            {
                userId = int.Parse(state.User.Claims.First(c => c.Type == Claims.UserId).Value);
            }

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
                account.Transactions = new ObservableCollection<Transaction>(await connection.Transactions.Where(t => t.AccountId == accountId).OrderByDescending(t => t.Timestamp).ToListAsync());
            }

            Accounts = accounts;
        }
    }
}
