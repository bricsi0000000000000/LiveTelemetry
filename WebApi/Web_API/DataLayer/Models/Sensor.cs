using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Sensor
    {
        [Key]
        public int SensorId { get; set; } // primary key

        [Required]
        public string Name { get; set; }
    }
}
