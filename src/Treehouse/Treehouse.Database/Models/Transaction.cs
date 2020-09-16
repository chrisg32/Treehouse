using System;
using System.ComponentModel.DataAnnotations;

namespace TreeHouse.Database.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int? OpposingTransactionId { get; set; }
        public Transaction OpposingTransaction { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }
}
