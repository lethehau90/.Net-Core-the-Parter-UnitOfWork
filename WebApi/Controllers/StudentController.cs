using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using AutoMapper;
using Model;
using WebApi.ViewModels;
using Data.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Student")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;
        public StudentController(IStudentService studentService, IUnitOfWork unitOfWork)
        {
            _studentService = studentService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public   async  Task<IEnumerable<Student>> GetAll()
        {
            //var student = _studentService.GetAll();
            //var studentModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(student);
            //return Ok(new { student = studentModel });

            var query = _unitOfWork.GetRepository<Student>();
            var values = await query.GetAll().ToListAsync();
            return values;
        }
    }
}