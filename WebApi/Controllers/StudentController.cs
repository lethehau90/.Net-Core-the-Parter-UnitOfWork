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
using Service.ViewModels;
using Common;
using Microsoft.Extensions.Options;

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
                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = queryPage,
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
                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = queryPage,
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
                return query;
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
                return Ok(query);
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

                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = query,
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
                var paginationSet = new PaginationSet<StudentViewModel>()
                {
                    Items = query,
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
        public IActionResult Insert([FromBody]StudentViewModel student)
        {
            try
            {
                if (_studentService.Find(student.Id) != null) return NoContent();
                _studentService.Insert(student);
                _unitOfWork.SaveChanges();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertAsync")]
        public async Task InsertAsync([FromBody]StudentViewModel student)
        {
            try
            {
                if (_studentService.Find(student.Id) != null) return;
                 _studentService.Insert(student);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        [HttpPut("UpdateAsync")]
        public async Task UpdateAsync([FromBody]StudentViewModel student)
        {
            try
            {
                if (_studentService.Find(student.Id) == null) return;
                _studentService.Update(student);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody]StudentViewModel student)
        {
            try
            {
                var query = _studentService.Find(student.Id);
                if (query == null) return NoContent();
                _studentService.Update(student);
                _unitOfWork.SaveChanges();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}