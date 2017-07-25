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

        [Fact]
        public void Create()
        {
            var mockStudentRepository = new Mock<IStudentRepository>();
            var mocUnitOfWork = new Mock<IUnitOfWork>();

            Student studentVm = new Student();
            studentVm = new Student
            {
                Id = 5,
                Name = "test 5"
            };

            mockStudentRepository.Setup(x => x.Insert(It.IsAny<Student>()));
            mockStudentRepository.Setup(x => x.Find(5)).Returns(studentVm);

            _studentRepository = mockStudentRepository.Object;
            _unitOfWork = mocUnitOfWork.Object;

            var _studentService = new StudentService(_studentRepository, _unitOfWork);

            //insert test
            _studentService.Insert(studentVm);

            //Find the test a value 5 
            var resurt = _studentService.Find(5);

            Assert.NotNull(resurt);
            Assert.Equal(5, resurt.Id);
        }

        [Fact]
        public void Update()
        {
            var mockStudentRepository = new Mock<IStudentRepository>();
            var mocUnitOfWork = new Mock<IUnitOfWork>();

            Student studentVm = new Student();
            studentVm = new Student
            {
                Id = 4,
                Name = "test 4"
            };

            mockStudentRepository.Setup(x => x.Insert(It.IsAny<Student>()));
            mockStudentRepository.Setup(x => x.Find(4)).Returns(studentVm);

            _studentRepository = mockStudentRepository.Object;
            _unitOfWork = mocUnitOfWork.Object;

            var _studentService = new StudentService(_studentRepository, _unitOfWork);

            //insert test
            _studentService.Update(studentVm);

            //Find the test a value 5 
            var resurt = _studentService.Find(4);

            Assert.NotNull(resurt);
            Assert.Equal(4, resurt.Id);
        }




    }
}
