using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KaromiProject.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^([A-Za-z]+)$", ErrorMessage = "Please Enter Valid Alphabets Only")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please Enter Valid Mobile Number")]
        public string Mobile { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Project { get; set; }

        public string Team { get; set; }

    }
    public class EmployeeLoginEmail
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Please Enter Valid Email")]
        public string LoginEmail { get; set; }
    }
}