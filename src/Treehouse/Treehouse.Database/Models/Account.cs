using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeHouse.Database.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Cash { get; set; }
        public bool Locked { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
