using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.ViewModels;

namespace MCSTask.FileOperations
{
    public interface IEmployeeCrudOperations
    {
        Task<List<Employee>> LoadEmployeesFromFile();
        Task<IEnumerable<Employee>> GetTargetEmps(string empType);
        Task<string> AddEmployee(EmployeeViewModel employee);
        IEnumerable<EmploymentType> GetEmploymentTypes();
        Task<IEnumerable<Employee>> DeleteEmployee(string name);
        Task<Employee> GetEmployeeByName(string name);
        Task WriteEmployeesToFile(IEnumerable<Employee> employees);
    }
}
