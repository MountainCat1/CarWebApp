using System.Collections.Generic;
using CarWebApp.Entities;

namespace CarWebApp.Models
{
    public class CarListModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public int? CarBrandFilerId { get; set; }
    }
}