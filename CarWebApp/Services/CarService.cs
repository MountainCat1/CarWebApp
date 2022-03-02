using System.Collections.Generic;
using System.Threading.Tasks;
using CarWebApp.Entities;

namespace CarWebApp.Services
{
    public interface ICarService
    {
        public Task<List<Car>> GetAll();
    }
}