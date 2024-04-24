using MCSExam.DTOs;
using MCSExam.Models;
using MCSTask.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace MCSExam.DataSource
{
    public interface IRepository
    {
        
        Task<ResultViewModel> GetAll(string? empType);
        Task<ResultViewModel> Create(EmployeeViewModel employee);
        IEnumerable<SelectListItem> GetEmploymentTypes();
        Task<ResultViewModel> Remove(string name);
        Task<ResultViewModel> GetEmployeeByName(string name);
    }
}
