namespace CarWebApp.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string VIN { get; set; }
        
        public virtual CarModel CarModel { get; set; }
        public int CarModelId { get; set; }
    }
}