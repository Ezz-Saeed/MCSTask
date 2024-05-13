using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.FileOperations;
using MCSTask.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Net;
using System.Xml.Linq;

namespace MCSExam.DataSource
{
    public class Repository : IRepository
    {
        private readonly IEmployeeCrudOperations _employeeCrudOperations;
        public Repository(IEmployeeCrudOperations employeeCrudOperations)
        {
            _employeeCrudOperations = employeeCrudOperations;
        }

        //Method uses GetTargetEmps method to filter the eployees returned by load method to
        //retrieve a list of data objects, message and status of retrieval
        public async Task<ResultViewModel> GetFilteredEmployees(string? empType)
        {
            var result = new ResultViewModel();
            try
            {
                var employees = await _employeeCrudOperations.GetTargetEmps(empType);
                result.Data = employees;
                result.Status = true;
                result.Message = "Seccess";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = false;
            }
            
            return result;
        }

        public async Task<ResultViewModel> GetAll()
        {
            var result = new ResultViewModel();
            try
            {
                var employees = await _employeeCrudOperations.LoadData();
                result.Data = employees;
                result.Status = true;
                result.Message = "Seccess";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = false;
            }

            return result;
        }

        //Uses AddEmployee method to extrct data from the view model and append new line to the file.
        public async Task<ResultViewModel> Create(EmployeeViewModel employee)
        {
            var result = new ResultViewModel();
            try
            {
                var emp = await _employeeCrudOperations.AddEmployee(employee);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = false;
            }
            result.Status = true;
            result.Data = employee;
            return result;
        }
        //Receives updated data of a specific employee to modify the line of this employee
        public async Task<ResultViewModel> Update(EmployeeViewModel employee)
        {
            var result = new ResultViewModel();
            try
            {
                 await _employeeCrudOperations.UpdateEmployee(employee);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = false;
            }
            result.Status = true;
            result.Data = employee;
            return result;
        }
        // Receives EmployeeViewModel of the line to be deleted
        public async Task<ResultViewModel> Remove(EmployeeViewModel model)
        {
            var result = new ResultViewModel();
            try
            {
                //method to remove the employee
                await _employeeCrudOperations.DeleteEmployee(model);
                result.Status = true;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Message = $"{ex.Message}";
                result.Status = false;
            }
            return result;
        }

        public async Task<ResultViewModel> GetEmployee(string name)
        {
            var result = new ResultViewModel();
            try
            {
                var emp = await _employeeCrudOperations.GetEmployee(name);
                result.Status = true;
                result.Message = "Success";
                result.Data = emp;
            }
            catch(Exception ex)
            {
                result.Message = ex.Message ;
                result.Status = false;
            }
            
            return result;
        }

        //To get SelectListItems of Employment Types to be displayed as a drop down list in the view.
        public IEnumerable<SelectListItem> GetEmploymentTypes()
        {
            var types = _employeeCrudOperations.GetEmploymentTypes();
            return types.Select(t => new SelectListItem{ Text = t.Name, Value = t.Id.ToString()}).OrderBy(t => t.Text).ToList();
        }

        //To get SelectListItems of Employment Assurance Types to be displayed as a drop down list in the view.
        public IEnumerable<SelectListItem> GetEmploymentAssurance()
        {
            var employmentAssurance = _employeeCrudOperations.GetEmploymentAssurance();
            return employmentAssurance.Select(a => new SelectListItem { Text=a.Value, Value=a.Id}).OrderBy(a => a.Text).ToList();
        }

        public async Task<ResultViewModel> FilterOutEmployees(decimal xValue)
        {
            var result = new ResultViewModel();
            try
            {
                var emps = await _employeeCrudOperations.FilterOutEmployees(xValue);
                result.Status = true;
                result.Message = "Success";
                result.Data = emps;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = false;
            }

            return result;
        }
    }
}
