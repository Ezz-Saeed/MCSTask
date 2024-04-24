using MCSExam.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCSTask.ViewModels
{
    public class EmployeeViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Graduation { get; set; } = string.Empty;
        //public EmploymentType? EmploymentType { get; set; } = default;
        public decimal Salary { get; set; } = default;
        public string? Assurance { get; set; } = string.Empty;
        //[ForeignKey(nameof(EmploymentType))]
        public string EmplomentTypeId { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> EmploymentTypes { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
