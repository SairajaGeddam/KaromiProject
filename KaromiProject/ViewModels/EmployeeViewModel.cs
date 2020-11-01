using KaromiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace KaromiProject.ViewModels
{
    public class EmployeeViewModel
    {
        public Employee Employee { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public EmployeeLoginEmail LoginEmail { get; set; }
        public List<string> Projects { get; set; }
        public List<string> Roles { get; set; }
    }
}