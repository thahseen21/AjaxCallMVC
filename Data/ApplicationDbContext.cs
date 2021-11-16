using AjaxCallMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AjaxCallMVC.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; }

    }
}