using System;
using System.Linq;
using TreeHouse.Database.Utility;

namespace TreehouseDatabaseCLI.Commands
{
    public class ResetPassword : BaseCommand
    {
        public string User { get; set; }
        public string NewPassword { get; set; }
        public ResetPassword() : base("rspass", "Reset password", "Reset a user's password.")
        {
            HasRequiredOption("u|user=", "The username (first name) of the user to reset.", u => User = u);
            HasRequiredOption("pw|password=", "The new password.", pw => NewPassword = pw);
        }

        protected override void Run()
        {
            using var connection = CreateConnection();
            var user = connection.Users.FirstOrDefault(u => u.FirstName == User);
            if(user == default) throw new Exception("Could not find user.");

            if(string.IsNullOrWhiteSpace(NewPassword)) throw new Exception("A password must be provided.");
            var hashedPass = PasswordHasher.HashPass(NewPassword);
            if (string.IsNullOrWhiteSpace(hashedPass)) throw new Exception("A password must be provided.");

            user.Password = hashedPass;
            connection.Users.Update(user);
            connection.SaveChanges();
            Console.WriteLine($"Password updated for user {user.FirstName} {user.LastName}.");
        }
    }
}
