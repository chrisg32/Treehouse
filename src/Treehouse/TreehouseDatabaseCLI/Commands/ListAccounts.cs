using System;
using System.Linq;

namespace TreehouseDatabaseCLI.Commands
{
    public class ListAccounts : BaseCommand
    {
        public ListAccounts() : base("la", "List accounts")
        {
        }

        protected override void Run()
        {
            using var connection = CreateConnection();
            var users = connection.Users.OrderBy(u => u.FirstName);
            foreach (var user in users)
            {
                foreach (var account in connection.Accounts.Where(a => a.UserId == user.Id))
                {
                    var lockedText = account.Locked ? " (locked)" : string.Empty;
                    var cashText = account.Cash ? " (cash)" : string.Empty;
                    Console.WriteLine($"{user.FirstName} {user.LastName} \t{account.Name}{lockedText}{cashText} \t{account.Description}");
                }

                Console.WriteLine();
            }
        }
    }
}
