using KaromiProject.Entities;
using KaromiProject.Utilities;
using KaromiProject.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace KaromiProject.Controllers
{
    public class EmployeeController : Controller
    {      
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            EmployeeViewModel employeeViewModel = Helper.CreateViewModelObj();
            employeeViewModel.Employees = KaromiDbContext.GetAllEmployees();
            Session.Clear();
            return View("Index", employeeViewModel);
        }

        //POST
        [HttpPost]
        public ActionResult Index(EmployeeViewModel emailObj)
        {
            if (!ModelState.IsValid)
            {
                return View("Index",emailObj);
            }
            else
            {
                Session["LoggedInEmail"] = emailObj.LoginEmail.LoginEmail;
                
                Models.Employee employee = KaromiDbContext.GetEmployeeProjectAndTeam(emailObj.LoginEmail.LoginEmail);
                if (employee != null)
                {
                    EmployeeViewModel empObj = Helper.GetEmployeesBasedOnRole(employee);
                    return View("List", empObj);
                }
                return View("Fail");
            }
        }

        // GET
        [HttpGet]
        public ActionResult List()
        {
            try
            {
                EmployeeViewModel emp = Helper.GetEmployeesBasedOnRole(KaromiDbContext.GetEmployeeProjectAndTeam(Session["LoggedInEmail"].ToString()));
                return View("List", emp);
            }
            catch (Exception) {
                return View("Fail");
            }
            
        }

        // POST
        [HttpPost]
        public ActionResult List(string Search)
        {
            EmployeeViewModel employeeViewModel = Helper.GetEmployeesBasedOnRole(KaromiDbContext.GetEmployeeProjectAndTeam(Session["LoggedInEmail"].ToString()));
            employeeViewModel.Employees = employeeViewModel.Employees.Where(emp => emp.Name.ToLower().Contains(Search.ToLower()));
            return View("List", employeeViewModel);
        }

        //GET
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", Helper.CreateViewModelObj());
        }

        //POST
        [HttpPost]
        public ActionResult Create(EmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid)
            {
                employeeViewModel.Roles = Helper.SetRoles();
                employeeViewModel.Projects = Helper.SetProjects();
                return View("Create", employeeViewModel);
            }
            else 
            {
                bool created = KaromiDbContext.CreateEmployee(employeeViewModel.Employee);
                if (created)
                {
                    EmployeeViewModel emp = Helper.GetEmployeesBasedOnRole(KaromiDbContext.GetEmployeeProjectAndTeam(Session["LoggedInEmail"].ToString()));
                    return View("List", emp);
                }
                else
                    return View("Fail");
            }
        }

        //GET
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.LoggedInEmployee = KaromiDbContext.GetEmployeeProjectAndTeam(Session["LoggedInEmail"].ToString());
            EmployeeViewModel emp = Helper.CreateViewModelObj();
            emp.Employee = KaromiDbContext.GetEmployeeProjectAndTeamById(id);
            return View("Edit", emp);
        }

        //POST
        [HttpPost]
        public ActionResult Edit(Models.Employee employee)
        {
            ViewBag.LoggedInEmployee = KaromiDbContext.GetEmployeeProjectAndTeam(Session["LoggedInEmail"].ToString());
            if (!ModelState.IsValid)
            {
                EmployeeViewModel employeeViewModel = Helper.CreateViewModelObj();
                employeeViewModel.Employee = employee;
                return View("Edit", employeeViewModel);
            }
            else
            {
                bool edited = KaromiDbContext.EditEmployee(employee);
                if (edited)
                {
                    EmployeeViewModel emp = Helper.GetEmployeesBasedOnRole(KaromiDbContext.GetEmployeeProjectAndTeam(Session["LoggedInEmail"].ToString()));
                    return View("List", emp);
                }
                else
                    return View("Fail");
            }
        }

        //GET
        [HttpGet]
        public ActionResult Delete(int id)
        {
            EmployeeViewModel emp = Helper.CreateViewModelObj();
            emp.Employee = KaromiDbContext.GetEmployeeProjectAndTeamById(id);
            return View("Delete", emp);
        }

        //POST
        [HttpPost]
        public ActionResult Delete(Models.Employee employee)
        {
            bool deleted = KaromiDbContext.DeleteEmployee(employee);
            if (deleted)
            {
                EmployeeViewModel emp = Helper.GetEmployeesBasedOnRole(KaromiDbContext.GetEmployeeProjectAndTeam(Session["LoggedInEmail"].ToString()));
                return View("List", emp);
            }
            else
                return View("Fail");
        }

        [HttpGet]
        public JsonResult DynamicTeams(string id)
        {
            return Json(KaromiDbContext.GetTeamsBasedOnProject(id), JsonRequestBehavior.AllowGet);
        }
    }
}