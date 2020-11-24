using System;
using System.Linq;

namespace TreehouseDatabaseCLI.Commands
{
    public class ListUsers : BaseCommand
    {
        public ListUsers() : base("lu", "List users")
        {
        }

        protected override void Run()
        {
            using var connection = CreateConnection();
            var users = connection.Users.OrderBy(u => u.FirstName);
            foreach (var user in users)
            {
                Console.WriteLine($"{user.FirstName} {user.LastName}");
            }
        }
    }
}
