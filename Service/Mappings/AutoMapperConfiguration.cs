using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using Service.ViewModels;

namespace Service.Mappings
{
    public class AutoMapperConfiguration : Profile
    {
        public override string ProfileName => "AutoMapperConfiguration";
        public AutoMapperConfiguration()
        {

            CreateMap<Student, StudentViewModel>();
            CreateMap<Course, CourseViewModel>();
            CreateMap<Enrollment, EnrollmentViewModel>();

        }
    }
}
