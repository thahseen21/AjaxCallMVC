using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AjaxCallMVC.Models;
using AjaxCallMVC.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AjaxCallMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _dbContext;

        List<Employee> EmployeeList = new List<Employee>();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            this.EmployeeList = this._dbContext.Employee.ToList();
            return View(this.EmployeeList);
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [NonAction]
        public void AddEmployee(Employee model)
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
                if (File == null)
                {
                    return BadRequest();
                }

                // To check whether the file is pdf and the size is less than 500kb 
                if (File.ContentType != "application/pdf" || !(File.Length < 500000) || !(EmployeeName.Length > 0))
                {
                    return BadRequest();
                }

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

                stream.Close();

                //The path of the incoming file is added with the employee entity
                var model = new Employee()
                {
                    EmployeeName = EmployeeName,
                    EmployeeDOB = EmployeeDOB,
                    ResumePath = pathToSave
                };

                this.AddEmployee(model);

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
