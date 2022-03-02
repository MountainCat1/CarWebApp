using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarWebApp.Entities;
using CarWebApp.Models;
using CarWebApp.Services;

namespace CarWebApp.Data
{
    public class DbSeeder
    {
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;

        public DbSeeder(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task Seed()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Users.Any())
                await SeedUsers();

            if (!_context.CarBrands.Any())
                await SeedCarBrands();
            
            if (!_context.CarModels.Any())
                await SeedCarModels();
            
            if (!_context.Cars.Any())
                await SeedCars();
        }

        private async Task SeedCars()
        {
            var cars = new List<Car>()
            {
                new Car()
                {
                    //Id = 1,
                    RegistrationNumber = "SO12406",
                    CarModelId = 1,
                    VIN = "9FBX8LD543D4P0871"
                },
                new Car()
                {
                    //Id = 2,
                    RegistrationNumber = "ZSL0977",
                    CarModelId = 1,
                    VIN = "5NMXNF374928V5697"
                },
                new Car()
                {
                    //Id = 3,
                    RegistrationNumber = "NKE2811",
                    CarModelId = 2,
                    VIN = "VF4PRUT06BVL26481"
                },
                new Car()
                {
                    //Id = 4,
                    RegistrationNumber = "POS9498",
                    CarModelId = 3,
                    VIN = "9BMXRS7Z3U81R1536"
                },
                new Car()
                {
                    //Id = 5,
                    RegistrationNumber = "KLI4124",
                    CarModelId = 4,
                    VIN = "LWVUGHXD720LJ9475"
                }
            };
            await _context.Cars.AddRangeAsync(cars);
            await _context.SaveChangesAsync();
        }

        private async Task SeedCarModels()
        {
            var carModels = new List<CarModel>()
            {
                new CarModel()
                {
                    //Id = 1,
                    Name = "Toyota Aygo X",
                    CarBrandId = 1,
                },
                new CarModel()
                {
                    //Id = 2,
                    Name = "Yaris GR SPORT",
                    CarBrandId = 1,
                },
                new CarModel()
                {
                    //Id = 3,
                    Name = "Fabia Combi",
                    CarBrandId = 2
                },
                new CarModel()
                {
                    //Id = 4,
                    Name = "Atlas Cross Sport",
                    CarBrandId = 3
                }
            };
            await _context.CarModels.AddRangeAsync(carModels);
            await _context.SaveChangesAsync();
        }

        private async Task SeedCarBrands()
        {
            var carBrands = new List<CarBrand>()
            {
                new CarBrand()
                {
                    //Id = 1,
                    Name = "Toyota",
                },
                new CarBrand()
                {
                    //Id = 2,
                    Name = "Skoda",
                },
                new CarBrand()
                {
                    //Id = 3,
                    Name = "Volkswagen"
                }
            };
            await _context.CarBrands.AddRangeAsync(carBrands);
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            await _userService.RegisterUser(new RegisterModel()
            {
                Username = "admin",
                Password = "admin"
            }, roleId: 1);
        }
    }
}