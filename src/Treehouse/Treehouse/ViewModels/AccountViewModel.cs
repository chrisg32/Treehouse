using System.Collections.Generic;
using System.Linq;
using TreeHouse.Database.Models;

namespace TreeHouse.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance => Transactions?.Any() == true ? Transactions.Sum(t => t.Amount) : default;
        public List<Transaction> Transactions { get; set; }
    }
}
