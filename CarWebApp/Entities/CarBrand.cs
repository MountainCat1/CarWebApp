using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebApp.Entities
{
    public class CarBrand
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Brand Name")]
        public string Name { get; set; }
    }
}