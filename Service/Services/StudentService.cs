using Model;
using Service.BaseInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Data.Repositories;
using AutoMapper;
using System.Threading.Tasks;
using Data.Infrastructure.UnitOfWork;
using Data.Infrastructure.PagedList;
using System.Data.SqlClient;
using Common.Store;

namespace Service.Services
{
    public interface IStudentService
    {
        IQueryable<Student> GetPagedList(string search, int pageindex, int pageSize);
        Task<IQueryable<Student>> GetPagedListAsync(string search, int pageIndex, int pageSize);
        IQueryable<Student> FromSql(string Id);
        IQueryable<StudentStore> GetModelFromQuery(string Id);
        Student Find(int Id);
        Task<Student> FindAsync(int Id);
        void Insert(Student student);
        Task InsertAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        void Delete(int Id);
        //Custom
        int Count(int Id);
        IQueryable<Student> GetAll();
        Student GetSingleByCondition(int Id);
        IQueryable<Student> GetMulti();
        IQueryable<Student> GetMultiPaging(int page, int pageSize, out int totalRow);
        bool CheckContains(int Id);
    }
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepository;
        private IUnitOfWork _unitOfWork;
        public StudentService(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public bool CheckContains(int Id)
        {
            return _studentRepository.CheckContains(x => x.Id.Equals(Id));
        }

        public int Count(int Id)
        {
            return _studentRepository.Count(x => x.Id.Equals(Id));
        }

        public void Delete(Student student)
        {
            var query = _studentRepository.Find(student.Id);
            if (query != null)
            {
                _studentRepository.Delete(query);
            }

        }

        public void Delete(int Id)
        {
            var query = _studentRepository.Find(Id);
            if (query != null)
            {
                _studentRepository.Delete(query);
            }
        }

        public Student Find(int Id)
        {
            var query = _studentRepository.Find(Id);
            return query;
        }

        public async Task<Student> FindAsync(int Id)
        {
            var query = await _studentRepository.FindAsync(Id);
            var queryModel = Mapper.Map<Student, Student>(query);
            return queryModel;
        }

        public IQueryable<Student> FromSql(string Id)
        {
            object[] sqlParams =
            {
                new SqlParameter("@Id", Id)
            };
            return _studentRepository.FromSql("getById @Id", sqlParams);
        }

        public IQueryable<Student> GetAll()
        {
            //include for <<new string[] {"Enrollments"}>>
            var query = _studentRepository.GetAll(null);
            return query.AsQueryable();
        }

        public IQueryable<Student> GetMulti()
        {
            var query = _studentRepository.GetMulti(x => true);
            return query.AsQueryable();
        }

        public IQueryable<Student> GetMultiPaging(int page, int pageSize, out int totalRow)
        {
            var query = _studentRepository.GetMultiPaging(null, out totalRow, page, pageSize);
            return query.AsQueryable();
        }

        public IQueryable<Student> GetPagedList(string search, int pageindex, int pageSize)
        {
            IEnumerable<Student> query;

            if (string.IsNullOrEmpty(search))
            {
                query = _studentRepository.GetPagedList(null, null, pageindex, pageSize).Items;
            }
            else
            {
                query = _studentRepository.GetPagedList(x => x.Name.Contains(search), null, pageindex, pageSize).Items;
            }
            return query.AsQueryable();
        }

        public async Task<IQueryable<Student>> GetPagedListAsync(string search, int pageIndex, int pageSize)
        {
            IPagedList<Student> query;

            if (string.IsNullOrEmpty(search))
            {
                query = await _studentRepository.GetPagedListAsync(null, null, null, pageIndex, pageSize);
            }
            else
            {
                query = await _studentRepository.GetPagedListAsync(x => x.Name.Contains(search), null, null, pageIndex, pageSize);
            }
            var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<Student>>(query.Items);
            return queryModel.AsQueryable();
        }

        public Student GetSingleByCondition(int Id)
        {
            var query = _studentRepository.GetSingleByCondition(x => x.Id.Equals(Id), new string[] { "Enrollments" });
            var queryModel = Mapper.Map<Student, Student>(query);
            return queryModel;
        }

        public void Insert(Student student)
        {
            _studentRepository.Insert(student);
        }

        public async Task InsertAsync(Student student)
        {
            await _studentRepository.InsertAsync(student);
        }

        public void Update(Student student)
        {
            _studentRepository.Update(student);
        }

        public IQueryable<StudentStore> GetModelFromQuery(string Id)
        {
            return _studentRepository.getModelFromQuery(Id);
        }
    }
}
