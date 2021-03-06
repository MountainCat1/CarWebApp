using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebApp.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Registration Number")]
        [StringLength(maximumLength: 7, MinimumLength = 7, ErrorMessage = "Registration number must have 7 characters.")]
        public string RegistrationNumber { get; set; }
        
        [Required]
        [StringLength(maximumLength: 17, MinimumLength = 17, ErrorMessage = "VIN must have 17 characters.")] 
        [DisplayName("VIN")] 
        public string VIN { get; set; }

        public virtual CarModel CarModel { get; set; }
        [DisplayName("Car Model")]
        public int CarModelId { get; set; }
    }
}