using MCSExam.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCSTask.ViewModels
{
    public class EmployeeViewModel
    {
        [Key]
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; } = string.Empty;
        public string Graduation { get; set; } = string.Empty;
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; } = default;
        [DataType(DataType.Currency)]
        public decimal OverTimeHourRate { get; set; } = default;
        [Display(Name = "Emploment Type")]
        public string EmplomentTypeId { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> EmploymentTypes { get; set; } = Enumerable.Empty<SelectListItem>();
        [Display(Name = "Assurance Type")]
        public string? EmplomentAssuranceId { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> EmploymentAssurance { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
