using System.Collections.Generic;
using System.Threading.Tasks;
using CarWebApp.Data;
using CarWebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWebApp.Services
{
    public interface ICarModelService
    {
        public Task<List<CarModel>> GetAll();
    }
    public class CarModelService : ICarModelService
    {
        private readonly DatabaseContext _context;

        public CarModelService(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<CarModel>> GetAll()
        {
            var carModels = _context.CarModels
                .Include(x => x.CarBrand);

            return await carModels.ToListAsync();
        }
    }
}