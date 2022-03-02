namespace CarWebApp.Entities
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual CarBrand CarBrand { get; set; }
        public int CarBrandId { get; set; }
    }
}