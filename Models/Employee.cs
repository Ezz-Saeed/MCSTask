using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCSExam.Models
{
    public class Employee
    {
        [Key]
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; } = string.Empty;
        public string Graduation { get; set; } = string.Empty;
        public string? Assurance { get; set; } = string.Empty;
        [Display(Name = "Salary")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; } = default;
        public EmploymentType EmploymentType { get; set; } = default;
        [ForeignKey(nameof(EmploymentType))]
        public string EmplomentTypeId { get; set; } = string.Empty;
        [Display(Name = "OverTime Hour Rate")]
        [DataType(DataType.Currency)]
        public decimal OverTimeHourRate { get; set; } = default;

        
    }
}


