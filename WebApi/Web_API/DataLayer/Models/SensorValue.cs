using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class SensorValue
    {
        [Key]
        public int SensorValueId { get; set; } // PK

        [Required]
        public double Value { get; set; }

        [Required]
        public int SensorId { get; set; } // FK

        [Required]
        public int SessionId { get; set; } // FK

        [Required]
        public int PackageId { get; set; }

        public virtual Sensor Sensor { get; set; } // required for foreign key
        public virtual Session Session { get; set; } // required for foreign key
    }
}
