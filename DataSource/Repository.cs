using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.FileOperations;
using MCSTask.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace MCSExam.DataSource
{
    public class Repository : IRepository
    {
        private readonly IEmployeeCrudOperations _employeeCrudOperations;
        public Repository(IEmployeeCrudOperations employeeCrudOperations)
        {
            _employeeCrudOperations = employeeCrudOperations;
        }

        //Method to retrieve a list of data objects, message and status of retrieval
        public async Task<ResultViewModel> GetAll(string empType)
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

        public async Task<ResultViewModel> Remove(string name)
        {
            var result = new ResultViewModel();
            try
            {
                var updatedEmployees = await _employeeCrudOperations.DeleteEmployee(name); // Example: Delete an employee named "John Doe"
                await _employeeCrudOperations.WriteEmployeesToFile(updatedEmployees); // Write the updated employee data to the file
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

        public async Task<ResultViewModel> GetEmployeeByName(string name)
        {
            var result = new ResultViewModel();
            try
            {
                var emp = await _employeeCrudOperations.GetEmployeeByName(name);
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

        public IEnumerable<SelectListItem> GetEmploymentTypes()
        {
            var types = _employeeCrudOperations.GetEmploymentTypes();
            return types.Select(t => new SelectListItem{ Text = t.Name, Value = t.Id.ToString()}).OrderBy(t => t.Text).ToList();
        }


    }
}
