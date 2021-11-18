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
            try
            {
                this._dbContext.Employee.Add(model);
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
                //the path to save the incoming file
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

                //Checking whether the folder exist or else create a folder using the pathToSave variable
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                //getting the file name from incoming file
                var fileName = File.FileName;

                //the final path where the file would save
                var fullPath = Path.Combine(pathToSave, fileName);

                // creates a stream for a file to read and write 
                var stream = new FileStream(fullPath, FileMode.Create);

                //writing the file to final directory
                File.CopyTo(stream);

                //The path of the incoming file is added with the employee entity
                var model = new Employee()
                {
                    EmployeeName = EmployeeName,
                    EmployeeDOB = EmployeeDOB,
                    ResumePath = pathToSave
                };

                this.Privacy(model);

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
