using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebApp.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [DisplayName("RegistrationNumber")]
        [StringLength(maximumLength: 7, MinimumLength = 7, ErrorMessage = "Registration number must have 7 characters.")]
        public string RegistrationNumber { get; set; }
        
        [StringLength(maximumLength: 17, MinimumLength = 17, ErrorMessage = "VIN must have 17 characters.")] 
        [DisplayName("VIN")] 
        public string VIN { get; set; }

        public virtual CarModel CarModel { get; set; }
        public int CarModelId { get; set; }
    }
}