using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCSExam.Models
{
    public class Employee
    {
        [Key]
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Graduation { get; set; } = string.Empty;
        public EmploymentType EmploymentType { get; set; } = default;
        [Display(Name ="Salary per hour")]
        public decimal Salary { get; set; } = default;
        public string? Assurance { get; set; } = string.Empty;
        [ForeignKey(nameof(EmploymentType))]
        public string EmplomentTypeId { get; set; } = string.Empty;
        [Display(Name = "OverTime Hour Rate")]
        public decimal OverTimeHourRate  => 
            EmplomentTypeId == "Hourly Payroll"? (Salary* 24*3)/16 : (EmplomentTypeId == "Monthly payroll" ? 
            ((Salary* 24 * 365) /160) : (Salary*1.5m));
        
    }
}


