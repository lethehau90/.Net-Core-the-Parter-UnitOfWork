using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using AutoMapper;
using Model;
using Data.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.Services;
using WebApi.ViewModels;
using Common;
using Microsoft.Extensions.Options;
using WebApi.EntityUpdateExtensions;
using Common.Store;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Student")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;
        private MySettings _mySettings { get; set; } //setting config the MyAPP

        public StudentController(
            IStudentService studentService,
            IUnitOfWork unitOfWork,
            IOptions<MySettings> mySettings)
        {
            _studentService = studentService;
            _unitOfWork = unitOfWork;
            _mySettings = mySettings.Value;
        }

        [HttpGet("FromSql")]
        public IActionResult FromSql(string Id)
        {
            try
            {
                IQueryable<Student> request = _studentService.FromSql(Id);
                var requestModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(request);
                return Ok(request.AsQueryable());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //connect Custom db prodedure
        [HttpGet("getModelFromQuery")]
        public IActionResult getModelFromQuery(string Id)
        {
            try
            {
                IQueryable<StudentStore> request = _studentService.getModelFromQuery(Id);
           
                return Ok(request.AsQueryable());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPagedList")]
        public IActionResult GetPagedList(string search, int page = 1, int pageSize = 20)
        {
            try
            {
                page = page > 0 ? page - 1 : page;
                int totalRow = 0;
                var queryAll = _studentService.GetAll();
                if (!string.IsNullOrEmpty(search))
                {
                    queryAll = queryAll.Where(x => x.Name.Contains(search));
                }
                totalRow = queryAll.Count();
                int totalPage = (int)Math.Ceiling((double)totalRow / (pageSize));

                var queryPage = _studentService.GetPagedList(search, page, pageSize).AsQueryable();
                var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(queryPage);
                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = queryModel.AsQueryable(),
                    MaxPage = _mySettings.MaxPage,
                    Page = page + 1,
                    TotalRow = totalRow,
                    TotalPage = totalPage
                };
                return Ok(paginationSet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPagedListAsync")]
        public async Task<PaginationSet<StudentViewModel>>  GetPagedListAsync(string search, int page = 1, int pageSize = 20)
        {
            try
            {
                page = page > 0 ? page - 1 : page;
                int totalRow = 0;
                var queryAll = _studentService.GetAll();
                if (!string.IsNullOrEmpty(search))
                {
                    queryAll = queryAll.Where(x => x.Name.Contains(search));
                }
                totalRow = queryAll.Count();
                int totalPage = (int)Math.Ceiling((double)totalRow / (pageSize));

                var queryPage = await  _studentService.GetPagedListAsync(search, page, pageSize);
                var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(queryPage);
                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = queryModel.AsQueryable(),
                    MaxPage = _mySettings.MaxPage,
                    Page = page + 1,
                    TotalRow = totalRow,
                    TotalPage = totalPage
                };

                return paginationSet;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("CheckContains")]
        public IActionResult CheckContains(int Id)
        {
            try
            {
                int count = _studentService.Count(Id);
                bool checkQuery = _studentService.CheckContains(Id);
                return Ok(new
                {
                    checkQuery,
                    count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Find")]
        public IActionResult Find(int Id)
        {
            try
            {
                var query = _studentService.Find(Id);
                var queryModel = Mapper.Map<Student, StudentViewModel>(query);
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FindAsync")]
        public async Task<StudentViewModel> FindAsync(int Id)
        {
            try
            {
                var query = await  _studentService.FindAsync(Id);
                var queryModel = Mapper.Map<Student, StudentViewModel>(query);
                return queryModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var query = _studentService.GetAll();
                var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query);
                return Ok(queryModel.AsQueryable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMulti")]
        public IActionResult GetMulti(string name, int page, int pageSize = 20)
        {
            try
            {
                page = page > 0 ? page - 1 : page;
                int totalRow = 0;
                var query = _studentService.GetMulti();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                totalRow = query.Count();
                int totalPage = (int)Math.Ceiling((Double)totalRow / pageSize);

                query = query.Skip((page) * pageSize).Skip(page);
                var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query);

                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = queryModel.AsQueryable(),
                    MaxPage = _mySettings.MaxPage,
                    Page = page + 1,
                    TotalRow = totalRow,
                    TotalPage = totalPage
                };

                return Ok(paginationSet);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMultiPaging")]
        public IActionResult GetMultiPaging(int page, int pageSize)
        {
            try
            {
                page = page > 0 ? page - 1 : page;
                int totalRow = 0;
                var query = _studentService.GetMultiPaging(page, pageSize, out totalRow);
                int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
                var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query);
                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = queryModel.AsQueryable(),
                    Page = page + 1,
                    TotalRow = totalRow,
                    TotalPage = totalPage,
                    MaxPage = _mySettings.MaxPage
                };
                return Ok(paginationSet);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSingleByCondition")]
        public IActionResult GetSingleByCondition(int Id)
        {
            try
            {
                var query = _studentService.GetSingleByCondition(Id);
                return Ok(query); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteEntity")]
        public IActionResult DeleteEntity(int Id)
        {
            try
            {
                var query = _studentService.Find(Id);
                if (query != null)
                {
                    _studentService.Delete(query);
                    _unitOfWork.SaveChanges();
                }
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var query = _studentService.Find(Id);
                _studentService.Delete(Id);
                _unitOfWork.SaveChanges();
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Insert")]
        public IActionResult Insert([FromBody]StudentViewModel studentVM)
        {
            try
            {
                if (_studentService.Find(studentVM.Id) != null) return NoContent();
                Student newStudent = new Student();
                newStudent.UpdateStudent(studentVM);
               _studentService.Insert(newStudent);
                _unitOfWork.SaveChanges();
                return Ok(newStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertAsync")]
        public async Task InsertAsync([FromBody]StudentViewModel studentVM)
        {
            try
            {
                if (_studentService.Find(studentVM.Id) != null) return;
                Student newStudent = new Student();
                newStudent.UpdateStudent(studentVM);
                 _studentService.Insert(newStudent);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        [HttpPut("UpdateAsync")]
        public async Task UpdateAsync([FromBody]StudentViewModel studentVM)
        {
            try
            {
                var query = _studentService.Find(studentVM.Id);
                if (query == null) return;
                query.UpdateStudent(studentVM);
                _studentService.Update(query);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody]StudentViewModel studentVM)
        {
            try
            {
                var query = _studentService.Find(studentVM.Id);
                if (query == null) return NoContent();
                query.UpdateStudent(studentVM);
                _studentService.Update(query);
                _unitOfWork.SaveChanges();
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}