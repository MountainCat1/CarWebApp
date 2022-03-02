using System.Collections.Generic;
using System.Threading.Tasks;
using CarWebApp.Data;
using CarWebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWebApp.Services
{
    public interface ICarService
    {
        Task<List<Car>> GetAll();
        Task Add(Car car);
        Task Edit(Car car);
        Task Edit(int carId);
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
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                await _context.Cars.AddAsync(car);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        public async Task Edit(Car car)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _context.Cars.Update(car);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        
        public async Task Edit(int carId)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var entity = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
                _context.Cars.Remove(entity);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
    }
}