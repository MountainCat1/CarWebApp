using System.Linq;
using System.Threading.Tasks;
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
            {
                await _userService.RegisterUser(new RegisterModel()
                {
                    Username = "admin",
                    Password = "admin"
                });
            }
        }
    }
}