using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class StudentViewModel
    {
        
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public virtual EnrollmentViewModel Enrollments { get; set; }
    }
}
