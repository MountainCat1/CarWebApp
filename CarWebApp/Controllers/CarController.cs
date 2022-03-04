using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarWebApp.Entities;
using CarWebApp.Exceptions;
using CarWebApp.Models;
using CarWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;


namespace CarWebApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICarModelService _carModelService;
        private readonly IUserService _userService;

        public CarController(ICarService carService, ICarModelService carModelService, IUserService userService)
        {
            _carService = carService;
            _carModelService = carModelService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery]int? carBrandFilerId)
        {
            var cars = await _carService.GetAll();

            if (carBrandFilerId != null)
            {
                cars = cars.Where(x => x.CarModel.CarBrand.Id == carBrandFilerId).ToList();
            }
            
            ViewBag.CarBrands = (await _carModelService.GetAll())
                .Select(x => x.CarBrand)
                .Distinct()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
            
            return View(new CarListModel()
            {
                Cars = cars,
                CarBrandFilerId = carBrandFilerId
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> Print([FromQuery]int? carBrandFilerId)
        {
            var model = await _carService.GetAll();

            if (carBrandFilerId != null)
            {
                model = model.Where(x => x.CarModel.CarBrand.Id == carBrandFilerId).ToList();
            }
            
            var pdf = new ViewAsPdf("PrintList", model);
            return pdf;
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var comboBoxList = await _carModelService.GetCarModelSelectList();
            ViewBag.CarModelId = comboBoxList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Car model)
        {
            if (await _userService.GetUser(this.User) is not {Role: UserRole.Admin})
            {
                throw new ForbidException("This action requires admin role");
            }
            
            await _carService.Add(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery]int id)
        {
            var comboBoxList = await _carModelService.GetCarModelSelectList();
            ViewBag.CarModelId = comboBoxList;

            var entity = await _carService.Get(id);
            
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Car model)
        {
            if (await _userService.GetUser(this.User) is not {Role: UserRole.Admin})
            {
                throw new ForbidException("This action requires admin role");
            }
            
            await _carService.Edit(model);
            
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteConfirm([FromQuery] int id)
        {
            return View( await _carService.Get(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]Car model)
        {
            if (await _userService.GetUser(this.User) is not {Role: UserRole.Admin})
            {
                throw new ForbidException("This action requires admin role");
            }
            
            await _carService.Remove(model.Id);
            
            return RedirectToAction("Index");
        }
    }
}