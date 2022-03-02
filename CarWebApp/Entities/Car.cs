using System.ComponentModel.DataAnnotations;

namespace CarWebApp.Entities
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string VIN { get; set; }
        
        public virtual CarModel CarModel { get; set; }
        public int CarModelId { get; set; }
    }
}