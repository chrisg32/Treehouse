using System;
using System.Linq;

namespace TreehouseDatabaseCLI.Commands
{
    public class ClearTransactions : BaseCommand
    {
        public ClearTransactions() : base("clearTrans", "Clear all transactions", "WARNING!!! - This command will delete all transactions in the database.")
        {
        }

        protected override void Run()
        {
            using var connection = CreateConnection();
            var all = connection.Transactions.ToList();
            Console.WriteLine($"Deleting {all.Count} transactions...");
            connection.Transactions.RemoveRange(all);
            connection.SaveChanges();
            Console.WriteLine($"All transactions have been deleted.");
        }
    }
}
