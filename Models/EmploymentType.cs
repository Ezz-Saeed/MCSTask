﻿using System.ComponentModel.DataAnnotations;

namespace MCSExam.Models
{
    public class EmploymentType
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        
    }
}
