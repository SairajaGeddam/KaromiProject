using KaromiProject.Entities;
using KaromiProject.Models;
using KaromiProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaromiProject.Utilities
{
    public static class Helper
    {
        public static EmployeeViewModel CreateViewModelObj()
        {
            EmployeeViewModel empObj = new EmployeeViewModel
            {
                Employee = new Models.Employee(),
                Projects = Helper.SetProjects(),
                Roles = Helper.SetRoles()
            };

            return empObj;
        }

        public static EmployeeViewModel GetEmployeesBasedOnRole(Models.Employee employee)
        {
            EmployeeViewModel empObj = Helper.CreateViewModelObj();
            var allEmployees = KaromiDbContext.GetAllEmployees();
            if (employee.Role.Equals(Enums.RoleEnums.HR.ToString()))
            {
                empObj.Employees = allEmployees;
                empObj.Employee = employee;
            }
            else if (employee.Role.Equals(Enums.RoleEnums.PM.ToString()))
            {
                var project = employee.Project;
                empObj.Employees = allEmployees.Where(emp => emp.Role != Enums.RoleEnums.HR.ToString() && emp.Project != null && emp.Project.Equals(project));
                empObj.Employee = employee;
            }
            else if (employee.Role.Equals(Enums.RoleEnums.TL.ToString()))
            {
                var team = employee.Team;
                empObj.Employees = allEmployees.Where(emp => emp.Role != Enums.RoleEnums.HR.ToString() && emp.Team != null && emp.Team.Equals(team));
                empObj.Employee = employee;
            }
            else if (employee.Role.Equals(Enums.RoleEnums.DEV.ToString()))
            {
                empObj.Employees = allEmployees.Where(emp => emp.Email.Equals(employee.Email));
                empObj.Employee = employee;
            }

            return empObj;
        }

        public static List<string> SetRoles()
        {
            List<string> roles = new List<string>();
            foreach (var role in KaromiDbContext.GetAllRoles())
            {
                roles.Add(role.RoleName);
            }
            return roles;
        }

        public static List<string> SetProjects()
        {
            List<string> projects = new List<string>();
            foreach (var project in KaromiDbContext.GetAllProjects())
            {
                projects.Add(project.ProjectName);
            }
            return projects;
        }
    }
}