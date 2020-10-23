using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeHouse.Database.Models;
using TreeHouse.Services;

namespace TreeHouse.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public AccountViewModel(DbService dbService) : base(dbService)
        {
        }

        private decimal _payment;
        private decimal _charge;
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance => Transactions?.Any() == true ? Transactions.Sum(t => t.Amount) : default;
        public List<Transaction> Transactions { get; set; }
        public User User { get; set; }

        public decimal Payment
        {
            get => _payment;
            set
            {
                if (value < 0) return;
                _payment = Math.Round(value, 2);
            }
        }

        public decimal Charge
        {
            get => _charge;
            set
            {
                if (value < 0) return;
                _charge = Math.Round(value, 2);
            }
        }

        public string PaymentDescription { get; set; }
        public string ChargeDescription { get; set; }

        public async Task SubmitPayment()
        {
            var tran = new Transaction
            {
                Amount = Payment,
                AccountId = Id,
                Timestamp = DateTime.Now,
                Description = PaymentDescription,
                CreatedById = User.Id
            };

            await SaveTransactionAsync(tran);
        }

        public async Task SubmitCharge()
        {
            var tran = new Transaction
            {
                Amount = Charge,
                AccountId = Id,
                Timestamp = DateTime.Now,
                Description = ChargeDescription,
                CreatedById = User.Id
            };

            await SaveTransactionAsync(tran);
        }

        private async Task SaveTransactionAsync(Transaction tran)
        {
            await using var connection = CreateConnection();

            var savedTran = await connection.Transactions.AddAsync(tran);
            await connection.SaveChangesAsync();

            Transactions.Insert(0, savedTran.Entity);
        }

    }
}
