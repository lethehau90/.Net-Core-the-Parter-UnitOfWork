using Model;
using Service.BaseInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Data.Repositories;

namespace Service
{
    public interface IStudentService : IBaseInterface<Student>
    {

    }
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Insert(Student t)
        {
            _studentRepository.Insert(t);
        }

        public void Delete(int Id)
        {
            var studentById = _studentRepository.GetSingleByCondition(x=>x.Id.Equals(Id));
            _studentRepository.Delete(studentById);
        }

        public void Update(Student t)
        {
            _studentRepository.Update(t);
        }

        public IQueryable<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public IQueryable<Student> GetAllPage(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            IQueryable<Student> query = _studentRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            totalRow = query.Count();
           return query.OrderBy(x => x.Name).Skip((page - 1)* pageSize).Take(pageSize);
        }

        public IQueryable<Student> GetById(int Id)
        {
            return _studentRepository.GetMulti(x=>x.Id.Equals(Id));
        }
    }
}
