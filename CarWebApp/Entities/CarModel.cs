using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebApp.Entities
{
    public class CarModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Model Name")]
        public string Name { get; set; }

        public virtual CarBrand CarBrand { get; set; }
        public int CarBrandId { get; set; }
    }
}