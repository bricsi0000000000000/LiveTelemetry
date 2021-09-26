using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Sensor
    {
        [Key]
        public int SensorId { get; set; } // PK

        [Required]
        public string Name { get; set; }
    }
}
