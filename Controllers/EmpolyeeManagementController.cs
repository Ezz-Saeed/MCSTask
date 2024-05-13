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

        //View to list Hourly Payroll employees.
        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork._repository.GetFilteredEmployees("Hourly Payroll");
            if (!result.Status)
                return BadRequest($"Something went woromg: {result.Message}");
             var hourlyPaidEmps = result.Data;
            return View(hourlyPaidEmps);
        }

        //View to list Hourly Payroll employees.
        public async Task<IActionResult> MonthlyPaidIndex()
        {
         
            var result = await _unitOfWork._repository.GetFilteredEmployees("Monthly payroll");
            if (!result.Status)
                return BadRequest($"Something went woromg: {result.Message}");
            var hourlyPaidEmps = result.Data;
            return View(hourlyPaidEmps);
        }

        //View to list Hourly Free lancers.
        public async Task<IActionResult> FreeLancerIndex()
        {
            var result = await _unitOfWork._repository.GetFilteredEmployees("Free lancer");
            if (!result.Status)
                return BadRequest($"Something went woromg: {result.Message}");
            var hourlyPaidEmps = result.Data;
            return View(hourlyPaidEmps);
        }

        //To view form to add new employee.
        [HttpGet]
        public IActionResult AddEmployee()
        {
            var types = _unitOfWork._repository.GetEmploymentTypes();
            var employmentAssurance = _unitOfWork._repository.GetEmploymentAssurance();
            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmploymentTypes = types, 
                EmploymentAssurance = employmentAssurance,
            };
            return View(employeeViewModel);
        }

        //Action to Insert data of new employee into the file.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> AddEmployee(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EmploymentTypes = _unitOfWork._repository.GetEmploymentTypes();
                model.EmploymentAssurance = _unitOfWork._repository.GetEmploymentAssurance();
                return View(model);
            }
            var addResult = await _unitOfWork._repository.GetAll();
            if (!addResult.Status) 
                return BadRequest($"Something went wrong: {addResult.Message}");
            var emps = addResult.Data as List<Employee>;
            var emp= emps.FirstOrDefault(e => e.Name == model.Name);
            if (emp != null)
                return BadRequest($"There is an employee with the name: {model.Name}");
            var result = await _unitOfWork._repository.Create(model);
            if (!result.Status) 
                return BadRequest($"Something went woromg: {result.Message}");
            return RedirectToAction(nameof(Index), "Home");
        }

        //View the data of the employee to be deleted.
        [HttpGet]
        public async Task<IActionResult> RemoveEmployee(string name)
        {
            var result = await _unitOfWork._repository.GetEmployee(name);
            var emp = result.Data as Employee;
            if (emp == null)
                return NotFound();
            var types = _unitOfWork._repository.GetEmploymentTypes();
            var employmentAssurance = _unitOfWork._repository.GetEmploymentAssurance();

            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmploymentTypes = types,
                Name = name,
                Address = emp.Address,
                BirthDate = emp.BirthDate,
                Graduation = emp.Graduation,
                EmplomentTypeId = emp.EmplomentTypeId,
                Salary = emp.Salary,
                EmplomentAssuranceId = emp.Assurance,
                EmploymentAssurance=employmentAssurance,
            };
            return View(employeeViewModel);
        }

        //Delete the line of an employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEmployee(EmployeeViewModel model)
        {
            var result = await _unitOfWork._repository.Remove(model);
            if(!result.Status)
                return BadRequest(result.Message);
            return RedirectToAction(nameof(Index), "Home");
        }

        //To view data of the employee to be updated and update it's data
        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(string name)
        {
            var result = await _unitOfWork._repository.GetEmployee(name);
            var emp = result.Data as Employee;
            if (emp == null)
            {
                return NotFound();
            }
                
            var types = _unitOfWork._repository.GetEmploymentTypes();
            var employmentAssurance = _unitOfWork._repository.GetEmploymentAssurance();
            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                EmploymentTypes = types,
                Name = name,
                Address = emp.Address,
                BirthDate = emp.BirthDate,
                Graduation = emp.Graduation,
                EmplomentTypeId = emp.EmploymentType.Id,
                Salary = emp.Salary,
                EmplomentAssuranceId = emp.Assurance,
                EmploymentAssurance = employmentAssurance
            };
            return View(employeeViewModel);
        }

        //Post the updated data to the file
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EmploymentTypes = _unitOfWork._repository.GetEmploymentTypes();
                model.EmploymentAssurance = _unitOfWork._repository.GetEmploymentAssurance();
                return View(model);
            }
            var result = await _unitOfWork._repository.Update(model);
            if (!result.Status)
                return BadRequest(result.Message);
            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
