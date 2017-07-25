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
using Service.ViewModels;

namespace XUnitTest
{
    public class UnitStudent
    {
      
        [Fact]
        public void GetAll()
        {
            var mockRepository = new Mock<IStudentRepository>();
           
            mockRepository.Setup(x => x.GetAll(null,null,true).ToList()).Returns(new List<Student>
            {
                    new Student(){ Id = 1 , Name = "SV1"},
                    new Student(){ Id = 2 , Name = "SV2"},
                    new Student(){ Id = 3 , Name = "SV3"},
                    new Student(){ Id = 4 , Name = "SV4"}
            });

            var mockService = new Mock<IStudentService>(mockRepository.Object);

            var request = mockService.Object.GetAll();

            Assert.NotNull(request.ToList());
            Assert.Equal(4, request.Count());

        }

   


    }
}
