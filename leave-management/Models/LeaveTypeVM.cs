using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }
        [Display(Name = "Test Name")]
        [Required]
        public string Name { get; set; }
        [Display(Name="Date Created")]
        public DateTime DateCreated { get; set; }
        [Required]
        [Display(Name = "Default Number of Days")]
        [Range(1, 25, ErrorMessage = "Please enter a valid number")]
        public int DefaultDays { get; set; }
    }
}
