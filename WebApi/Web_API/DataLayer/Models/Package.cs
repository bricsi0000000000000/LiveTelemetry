using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Package
    {
        [Required]
        public int PackageId { get; set; }

        [Required]
        public int SessionId { get; set; } // FK

        [Required]
        public long SentTime { get; set; }

        public virtual Session Session { get; set; } // required for foreign key
    }
}
