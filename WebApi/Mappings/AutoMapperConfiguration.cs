using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using WebApi.ViewModels;
using WebApi.ViewModels.System;

namespace WebApi.Mappings
{
    public class AutoMapperConfiguration : Profile
    {
        public override string ProfileName => "AutoMapperConfiguration";
        public AutoMapperConfiguration()
        {
            CreateMap<Student, StudentViewModel>();
            CreateMap<Course, CourseViewModel>();
            CreateMap<Enrollment, EnrollmentViewModel>();
            CreateMap<AppUser, RegisterViewModel>();
            CreateMap<AppUser, CredentialsViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<Permission, PermissionViewModel>();
        }
    }
}
