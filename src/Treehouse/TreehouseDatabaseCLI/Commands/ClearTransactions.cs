using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

            var dbConnection = connection.Database.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = "UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Transactions';";
            command.ExecuteNonQuery();

            Console.WriteLine($"All transactions have been deleted.");
        }
    }
}
