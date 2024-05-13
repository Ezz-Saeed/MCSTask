using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.Models;
using MCSTask.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCSTask.FileOperations
{
    public interface IEmployeeCrudOperations
    {
        Task<List<Employee>> LoadData();
        Task<IEnumerable<Employee>> GetTargetEmps(string empType);
        Task<string> AddEmployee(EmployeeViewModel employee);
        IEnumerable<EmploymentType> GetEmploymentTypes();
        IEnumerable<EmploymentAssurance> GetEmploymentAssurance();
        Task<IEnumerable<Employee>> DeleteEmployee(EmployeeViewModel model);
        Task<Employee> GetEmployee(string name);
        Task ReWriteEmployeesToFile(IEnumerable<Employee> employees);
        Task UpdateEmployee(EmployeeViewModel employee);
        Task<List<Employee>> FilterOutEmployees(decimal xValue);
    }
}
