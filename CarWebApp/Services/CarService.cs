using System.Collections.Generic;
using System.Threading.Tasks;
using CarWebApp.Data;
using CarWebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWebApp.Services
{
    public interface ICarService
    {
        public Task<List<Car>> GetAll();
        Task Add(Car car);
    }

    public class CarService : ICarService
    {
        private readonly DatabaseContext _context;

        public CarService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAll()
        {
            var cars = _context.Cars
                .Include(x => x.CarModel)
                .ThenInclude(x => x.CarBrand);

            return await cars.ToListAsync();
        }

        public async Task Add(Car car)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                await _context.Cars.AddAsync(car);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
    }
}