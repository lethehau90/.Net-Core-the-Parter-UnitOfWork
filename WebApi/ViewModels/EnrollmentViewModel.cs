using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class EnrollmentViewModel
    {
        
        public int Id { get; set; }
        public Grade? Grade { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int StudentId { get; set; }

        public virtual CourseViewModel Course { get; set; }

        public virtual StudentViewModel Student { get; set; }
    }
}
