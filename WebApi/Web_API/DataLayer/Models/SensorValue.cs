using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class SensorValue
    {
        [Key]
        public int SensorValueId { get; set; } // primary key

        [Required]
        public double Value { get; set; }

        [Required]
        public int SensorId { get; set; } // foreign key

        [Required]
        public int SessionId { get; set; } // foreign key

        [Required]
        public int PackageId { get; set; }

        public virtual Sensor Sensor { get; set; } // required for foreign key
        public virtual Session Session { get; set; } // required for foreign key
    }
}
