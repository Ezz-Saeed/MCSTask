using System.ComponentModel.DataAnnotations;

namespace MCSTask.Models
{
    public class EmploymentAssurance
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
