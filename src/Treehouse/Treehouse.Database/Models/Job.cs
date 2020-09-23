using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreeHouse.Database.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<JobComment> Comments { get; set; }
    }

    public class JobComment
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        [Required]
        public string Comment { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
