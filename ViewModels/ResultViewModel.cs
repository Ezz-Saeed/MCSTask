using MCSExam.Models;

namespace MCSExam.DTOs
{
    public class ResultViewModel
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public Object? Data { get; set; }
    }
}
