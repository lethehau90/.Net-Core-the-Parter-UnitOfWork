using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class CourseViewModel
    {
    
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        public int Credit { get; set; }

        public virtual ICollection<EnrollmentViewModel> Enrollments { get; set; }
    }
}
