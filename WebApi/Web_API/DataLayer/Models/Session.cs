using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; } // PK

        public bool IsLive { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
