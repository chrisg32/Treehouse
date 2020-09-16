using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeHouse.Database.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsParent { get; set; }
        public List<Account> Accounts { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
