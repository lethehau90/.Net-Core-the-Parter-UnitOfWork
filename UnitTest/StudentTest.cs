using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Data.Repositories;
using Data.Infrastructure.UnitOfWork;
using Model;
using Service.Services;

namespace UnitTest
{
    [TestClass]
    public class StudentTest
    {
        private Mock<IStudentRepository> _mocRepository;
        private Mock<IUnitOfWork> _mocUnitOfwork;
        private Mock<IStudentService> _mocService;
        private List<Student> _listStudent;

        [TestInitialize]
        public void Initialize()
        {
            _mocRepository = new Mock<IStudentRepository>();
            _mocUnitOfwork = new Mock<IUnitOfWork>();
            _mocService = new Mock<IStudentService>();
            _listStudent = new List<Student>()
            {
                new Student(){ Id = 1 , Name = "SV1"},
                new Student(){ Id = 2 , Name = "SV2"},
                new Student(){ Id = 3 , Name = "SV3"},
                new Student(){ Id = 4 , Name = "SV4"},
            };
        }

        [TestMethod]
        public void GetAll()
        {
            //set method
            _mocRepository.Setup(m => m.GetAll()).Throws(new Exception());
        }

    }
}
