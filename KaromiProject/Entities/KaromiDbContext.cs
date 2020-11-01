using KaromiProject.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KaromiProject.Entities
{
    public static class KaromiDbContext
    {
        private static karomiEntities _db = new karomiEntities();

        public static List<Models.Employee> GetAllEmployees()
        {
            List<Models.Employee> employeeList = new List<Models.Employee>();
            foreach (var employee in _db.Employees)
            {
                Models.Employee empObj = new Models.Employee();
                empObj.EmployeeId = employee.EmployeeId;
                empObj.Email = employee.Email;
                empObj.Name = employee.Name;
                empObj.Mobile = employee.MobileNumber;
                empObj.Role = _db.Roles.FirstOrDefault(role => role.RoleId == employee.RoleId).RoleName.ToString();
                empObj.Project = (_db.Projects.FirstOrDefault(proj => proj.ProjectId == (
                    _db.EmployeeProjTeamXrefs.FirstOrDefault(ept => ept.EmployeeId == employee.EmployeeId && ept.ProjectId!=null).ProjectId)))?.ProjectName;
                empObj.Team = (_db.Teams.FirstOrDefault(team => team.TeamId == (
                    _db.EmployeeProjTeamXrefs.FirstOrDefault(ept => ept.EmployeeId == employee.EmployeeId && ept.TeamId!=null).TeamId)))?.TeamName;

                employeeList.Add(empObj);
            }
            return employeeList;
        }

        public static Models.Employee GetEmployeeProjectAndTeam(string email)
        {
            Models.Employee empObj = new Models.Employee();

            var employee = _db.Employees.FirstOrDefault(emp => emp.Email.Equals(email));
                empObj.EmployeeId = employee.EmployeeId;
                empObj.Email = employee.Email;
                empObj.Name = employee.Name;
                empObj.Mobile = employee.MobileNumber;
                empObj.Role = _db.Roles.FirstOrDefault(role => role.RoleId == employee.RoleId).RoleName.ToString();
                empObj.Project = (_db.Projects.FirstOrDefault(proj => proj.ProjectId == (
                    _db.EmployeeProjTeamXrefs.FirstOrDefault(ept => ept.EmployeeId == employee.EmployeeId && ept.ProjectId != null).ProjectId)))?.ProjectName;
                empObj.Team = (_db.Teams.FirstOrDefault(team => team.TeamId == (
                    _db.EmployeeProjTeamXrefs.FirstOrDefault(ept => ept.EmployeeId == employee.EmployeeId && ept.TeamId != null).TeamId)))?.TeamName;

            return empObj;
        }

        public static Models.Employee GetEmployeeProjectAndTeamById(int id)
        {
            Models.Employee empObj = new Models.Employee();

            var employee = _db.Employees.FirstOrDefault(emp => emp.EmployeeId.Equals(id));
            empObj.EmployeeId = employee.EmployeeId;
            empObj.Email = employee.Email;
            empObj.Name = employee.Name;
            empObj.Mobile = employee.MobileNumber;
            empObj.Role = _db.Roles.FirstOrDefault(role => role.RoleId == employee.RoleId).RoleName.ToString();
            empObj.Project = (_db.Projects.FirstOrDefault(proj => proj.ProjectId == (
                _db.EmployeeProjTeamXrefs.FirstOrDefault(ept => ept.EmployeeId == employee.EmployeeId && ept.ProjectId != null).ProjectId)))?.ProjectName;
            empObj.Team = (_db.Teams.FirstOrDefault(team => team.TeamId == (
                _db.EmployeeProjTeamXrefs.FirstOrDefault(ept => ept.EmployeeId == employee.EmployeeId && ept.TeamId != null).TeamId)))?.TeamName;

            return empObj;
        }

        public static bool CreateEmployee(Models.Employee employee)
        {
            try
            {
                Entities.Employee empObj = new Entities.Employee();
                empObj.Email = employee.Email;
                empObj.Name = employee.Name;
                empObj.MobileNumber = employee.Mobile;
                empObj.RoleId = _db.Roles.FirstOrDefault(role => role.RoleName.Equals(employee.Role)).RoleId;

                _db.Employees.Add(empObj);
                _db.SaveChanges();

                employee.EmployeeId = _db.Employees.FirstOrDefault(emp => emp.Email.Equals(employee.Email)).EmployeeId;
                CreateEmployeeProjTeamXref(employee);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool CreateEmployeeProjTeamXref(Models.Employee employee)
        {
            try
            {
                EmployeeProjTeamXref employeeProjTeamXref = new EmployeeProjTeamXref();
                employeeProjTeamXref.EmployeeId = employee.EmployeeId;
                employeeProjTeamXref.ProjectId = _db.Projects.FirstOrDefault(proj => proj.ProjectName.Equals(employee.Project))?.ProjectId;
                employeeProjTeamXref.TeamId = _db.Teams.FirstOrDefault(team => team.TeamName.Equals(employee.Team))?.TeamId;

                _db.EmployeeProjTeamXrefs.Add(employeeProjTeamXref);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool EditEmployee(Models.Employee employee)
        {
            try
            {
                Entities.Employee empObj = _db.Employees.FirstOrDefault(emp => emp.EmployeeId == employee.EmployeeId);
                empObj.Email = employee.Email;
                empObj.Name = employee.Name;
                empObj.MobileNumber = employee.Mobile;
                empObj.RoleId = _db.Roles.FirstOrDefault(role => role.RoleName.Equals(employee.Role)).RoleId;
                
                _db.SaveChanges();

                EditEmployeeProjTeamXref(employee);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool EditEmployeeProjTeamXref(Models.Employee employee)
        {
            try
            {
                EmployeeProjTeamXref employeeProjTeamXref = _db.EmployeeProjTeamXrefs.FirstOrDefault(emp => emp.EmployeeId == employee.EmployeeId);
                employeeProjTeamXref.EmployeeId = employee.EmployeeId;
                employeeProjTeamXref.ProjectId = _db.Projects.FirstOrDefault(proj => proj.ProjectName.Equals(employee.Project))?.ProjectId;
                employeeProjTeamXref.TeamId = _db.Teams.FirstOrDefault(team => team.TeamName.Equals(employee.Team))?.TeamId;

                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteEmployee(Models.Employee employee)
        {
            try
            {
                DeleteEmployeeProjTeamXref(employee);

                Entities.Employee empObj = new Entities.Employee();
                empObj.EmployeeId = employee.EmployeeId;
                empObj.Email = employee.Email;
                empObj.Name = employee.Name;
                empObj.MobileNumber = employee.Mobile;
                empObj.RoleId = _db.Roles.FirstOrDefault(role => role.RoleName.Equals(employee.Role)).RoleId;

                _db.Employees.Remove(empObj);
                _db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool DeleteEmployeeProjTeamXref(Models.Employee employee)
        {
            try
            {
                EmployeeProjTeamXref employeeProjTeamXref = new EmployeeProjTeamXref();
                employeeProjTeamXref.EmployeeId = employee.EmployeeId;
                employeeProjTeamXref.ProjectId = _db.Projects.FirstOrDefault(proj => proj.ProjectName.Equals(employee.Project))?.ProjectId;
                employeeProjTeamXref.TeamId = _db.Teams.FirstOrDefault(team => team.TeamName.Equals(employee.Team))?.TeamId;
                employeeProjTeamXref.EmployeeProjXrefId = _db.EmployeeProjTeamXrefs.FirstOrDefault(eptXref => eptXref.EmployeeId == employeeProjTeamXref.EmployeeId && 
                                                            employeeProjTeamXref.ProjectId != null && eptXref.ProjectId == employeeProjTeamXref.ProjectId &&
                                                            employeeProjTeamXref.TeamId != null && eptXref.TeamId == employeeProjTeamXref.TeamId).EmployeeProjXrefId;

                _db.EmployeeProjTeamXrefs.Remove(employeeProjTeamXref);
                _db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<Entities.Project> GetAllProjects()
        {
            return _db.Projects.ToList();
        }

        public static List<string> GetTeamsBasedOnProject(string ProjectName)
        {
            List<string> teams = new List<string>();
            if (ProjectName != "")
            {
                int ProjectId = _db.Projects.FirstOrDefault(proj => proj.ProjectName.Equals(ProjectName)).ProjectId;
                foreach (var team in _db.Teams.Where(team => team.ProjectId == ProjectId).ToList())
                {
                    teams.Add(team.TeamName);
                }
            }
            return teams;
        }

        public static List<Entities.Role> GetAllRoles()
        {
            return _db.Roles.ToList();
        }
    }
}