using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWebApp.Data;
using CarWebApp.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarWebApp.Services
{
    public interface ICarModelService
    {
        public Task<List<CarModel>> GetAll();
        Task<List<SelectListItem>> GetCarModelSelectList();
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

        public async Task<List<SelectListItem>> GetCarModelSelectList()
        {
            var carModels = await GetAll();
            var comboBoxList = carModels
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            return comboBoxList;
        }
    }
}