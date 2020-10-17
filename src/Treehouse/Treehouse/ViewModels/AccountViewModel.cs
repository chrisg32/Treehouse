using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeHouse.Database.Models;

namespace TreeHouse.ViewModels
{
    public class AccountViewModel
    {
        private double _payment;
        private double _charge;
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance => Transactions?.Any() == true ? Transactions.Sum(t => t.Amount) : default;
        public List<Transaction> Transactions { get; set; }
        public string AccountOwnerName { get; set; }

        public double Payment
        {
            get => _payment;
            set
            {
                if (value < 0) return;
                _payment = Math.Round(value, 2);
            }
        }

        public double Charge
        {
            get => _charge;
            set
            {
                if (value < 0) return;
                _charge = Math.Round(value, 2);
            }
        }

        public async Task SubmitPayment()
        {
            
        }

        public async Task SubmitCharge()
        {

        }
    }
}
