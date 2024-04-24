using MCSExam.DataSource;
using MCSExam.DTOs;
using MCSExam.FileOperations;
using MCSExam.Models;
using MCSTask.FileOperations;
using MCSTask.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MCSExam.Controllers
{
    public class EmpolyeeManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmpolyeeManagementController(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork._repository.GetAll("Hourly Payroll");
            if (!result.Status)
                return BadRequest($"Something went woromg: {result.Message}");
             var hourlyPaidEmps = result.Data;
            return View(hourlyPaidEmps);
        }



        public async Task<IActionResult> MonthlyPaidIndex()
        {
         
            var result = await _unitOfWork._repository.GetAll("Monthly payroll");
            if (!result.Status)
                return BadRequest($"Something went woromg: {result.Message}");
            var hourlyPaidEmps = result.Data;
            return View(hourlyPaidEmps);
        }


        public async Task<IActionResult> FreeLancerIndex()
        {
            var result = await _unitOfWork._repository.GetAll("Free lancer");
            if (!result.Status)
                return BadRequest($"Something went woromg: {result.Message}");
            var hourlyPaidEmps = result.Data;
            return View(hourlyPaidEmps);
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            var types = _unitOfWork._repository.GetEmploymentTypes();
            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmploymentTypes = types, 
            };
            return View(employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> AddEmployee(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                employee.EmploymentTypes = _unitOfWork._repository.GetEmploymentTypes();
                return View(employee);
            }
                
            var result = await _unitOfWork._repository.Create(employee);
            if (!result.Status) 
                return BadRequest($"Something went woromg: {result.Message}");
            return RedirectToAction(nameof(Index), "Home");
        }
        [HttpGet]
        public async Task<IActionResult> RemoveEmployee(string name)
        {
            var result = await _unitOfWork._repository.GetEmployeeByName(name);
            var emp = result.Data as Employee;
            if (emp == null)
                return NotFound();
            var types = _unitOfWork._repository.GetEmploymentTypes();
            
            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmploymentTypes = types,
                Name = name,
                Address = emp.Address,
                BirthDate = emp.BirthDate,
                Graduation = emp.Graduation,
                EmplomentTypeId = emp.EmplomentTypeId,
                Salary = emp.Salary,
                Assurance = emp.Assurance,
            };
            return View(employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEmployee(EmployeeViewModel model)
        {
            var result = await _unitOfWork._repository.Remove(model.Name);
            if(!result.Status)
                return BadRequest(result.Message);
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(string name)
        {
            var result = await _unitOfWork._repository.GetEmployeeByName(name);
            var emp = result.Data as Employee;
            if (emp == null)
                return NotFound();
            var types = _unitOfWork._repository.GetEmploymentTypes();

            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmploymentTypes = types,
                Name = name,
                Address = emp.Address,
                BirthDate = emp.BirthDate,
                Graduation = emp.Graduation,
                EmplomentTypeId = emp.EmplomentTypeId,
                Salary = emp.Salary,
                Assurance = emp.Assurance,
            };
            return View(employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(EmployeeViewModel model)
        {
            var result = await _unitOfWork._repository.Remove(model.Name);
            if (!result.Status)
                return BadRequest(result.Message);
            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
