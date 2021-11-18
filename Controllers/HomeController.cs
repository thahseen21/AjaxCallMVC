using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AjaxCallMVC.Models;
using AjaxCallMVC.Data;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace AjaxCallMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _dbContext;

        List<Employee> EmployeeList = new List<Employee> {
                                new Employee() { EmployeeId = 1, EmployeeName = "potato" },
                                new Employee { EmployeeId = 2, EmployeeName = "cabbage" },
                                new Employee { EmployeeId = 3, EmployeeName = "beetroot" }
                            };

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(this.EmployeeList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public void Privacy(Employee model)
        {
            this.EmployeeList.Add(model);


            try
            {
                Employee entity = new Employee() { EmployeeName = model.EmployeeName };
                this._dbContext.Employee.Add(entity);
                this._dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                // TODO
            }
        }

        [HttpPost]
        public IActionResult Upload(string EmployeeName, DateTime EmployeeDOB, IFormFile File)
        {
            try
            {
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

                var fileName = File.FileName;

                var fullPath = Path.Combine(pathToSave, fileName);

                var stream = new FileStream(fullPath, FileMode.Create);

                File.CopyTo(stream);

                var model = new Employee()
                {
                    EmployeeName = EmployeeName,
                    EmployeeDOB = EmployeeDOB
                };

                this.Privacy(model);

                // return Ok();
                return Ok();

            }
            catch (System.Exception ex)
            {
                // TODO
                return BadRequest();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
