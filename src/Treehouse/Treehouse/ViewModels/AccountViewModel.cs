using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Transaction> Transactions { get; set; }
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
        public DateTime PaymentDate { get; set; }
        public DateTime ChargeDate { get; set; }

        public async Task SubmitPayment()
        {
            var tran = new Transaction
            {
                Amount = Payment,
                AccountId = Id,
                Timestamp = DateTime.Today == PaymentDate ? DateTime.Now : PaymentDate,
                Description = PaymentDescription,
                CreatedById = User.Id
            };

            await SaveTransactionAsync(tran);
        }

        public async Task SubmitCharge()
        {
            var tran = new Transaction
            {
                Amount = -Math.Abs(Charge),
                AccountId = Id,
                Timestamp = DateTime.Today == ChargeDate ? DateTime.Now : ChargeDate,
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
