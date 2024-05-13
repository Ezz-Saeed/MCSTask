using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace MCSExam.DataSource
{
    public interface IRepository
    {
        Task<ResultViewModel> GetAll();
        Task<ResultViewModel> GetFilteredEmployees(string? empType);
        Task<ResultViewModel> Create(EmployeeViewModel employee);
        IEnumerable<SelectListItem> GetEmploymentTypes();
        IEnumerable<SelectListItem> GetEmploymentAssurance();
        Task<ResultViewModel> Remove(EmployeeViewModel model);
        Task<ResultViewModel> GetEmployee(string name);
        Task<ResultViewModel> Update(EmployeeViewModel employee);
        Task<ResultViewModel> FilterOutEmployees(decimal xValue);
    }
}
