using System.ComponentModel.DataAnnotations;

namespace MCSExam.Models
{
    public class EmploymentType
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        
    }
}
