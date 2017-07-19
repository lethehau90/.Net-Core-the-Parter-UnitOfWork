using Model;
using Service.BaseInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Data.Repositories;

namespace Service
{
    public interface IStudentService 
    {
        IQueryable<Student> Query();
        IQueryable<Student> GetPagedList(string search, int pageindex, int pageSize);
        IQueryable<Student> GetPagedListAsync(string search, int pageIndex, int pageSize);
        IQueryable<Student> FromSql(string sql, string Id);
        

    }
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IQueryable<Student> FromSql(string sql, string Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Student> GetPagedList(string search, int pageindex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Student> GetPagedListAsync(string search, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Student> Query()
        {
            throw new NotImplementedException();
        }
    }
}
