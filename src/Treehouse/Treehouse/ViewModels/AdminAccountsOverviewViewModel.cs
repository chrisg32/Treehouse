using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeHouse.Database.Models;
using TreeHouse.Services;

namespace TreeHouse.ViewModels
{
    public class AdminAccountsOverviewViewModel : ViewModelBase
    {
        public AdminAccountsOverviewViewModel(DbService dbService) : base(dbService) { }
        public List<User> Users { get; private set; }

        public async Task LoadAsync()
        {
            await using var db = CreateConnection();
            Users = await db.Users.Where(u => u.IsParent == false).OrderBy(u => u.FirstName).ToListAsync();
        }
    }
}
