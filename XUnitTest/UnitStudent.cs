using Data.Infrastructure.UnitOfWork;
using Data.Repositories;
using Model;
using Service.Services;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using NSubstitute;
using System.Linq;
using WebApi.ViewModels;
using AutoMapper;
using WebApi.Mappings;

namespace XUnitTest
{
    public class UnitStudent
    {
        private IStudentRepository _studentRepository;
        private IUnitOfWork _unitOfWork;
        public List<Student> _listStudent = new List<Student>()
            {
                    new Student() { Id = 1 , Name = "SV1"},
                    new Student() { Id = 2 , Name = "SV2"},
                    new Student() { Id = 3 , Name = "SV3"},
                    new Student() { Id = 4 , Name = "SV4"}
            };

        public UnitStudent()
        {
            Mapper.Initialize(x => x.AddProfile<AutoMapperConfiguration>());
        }

        [Fact]
        public void GetAll()
        {
            var mockStudentRepository = new Mock<IStudentRepository>();
            var mocUnitOfWork = new Mock<IUnitOfWork>();

            mockStudentRepository.Setup(x => x.GetAll(null, null, true)).Returns(_listStudent.AsQueryable());

            _studentRepository = mockStudentRepository.Object;
            _unitOfWork = mocUnitOfWork.Object;

            var _studentService = new StudentService(_studentRepository, _unitOfWork);

            var request = _studentService.GetAll();

            Assert.NotNull(request.ToList());
            Assert.Equal(4, request.Count());
        }

        public void Create()
        {
            var mockStudentRepository = new Mock<IStudentRepository>();
            var mocUnitOfWork = new Mock<IUnitOfWork>();

            Student studentVm = new Student();
            studentVm = new Student
            {
                Id = 4,
                Name = "test 4"
            };

            mockStudentRepository.Setup(x => x.Insert());

            var _studentService = new StudentService(_studentRepository, _unitOfWork);
            _studentService.Insert(studentVm);

        }




    }
}
