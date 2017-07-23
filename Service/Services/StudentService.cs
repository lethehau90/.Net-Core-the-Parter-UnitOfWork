using Model;
using Service.BaseInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Data.Repositories;
using Service.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using Data.Infrastructure.UnitOfWork;
using Data.Infrastructure.PagedList;
using Service.EntityUpdateExtensions;

namespace Service.Services
{
    public interface IStudentService
    {
        IQueryable<StudentViewModel> GetPagedList(string search, int pageindex, int pageSize);
        Task<IQueryable<StudentViewModel>> GetPagedListAsync(string search, int pageIndex, int pageSize);
        IQueryable<StudentViewModel> FromSql(string sql, string Id);
        StudentViewModel Find(int Id);
        Task<StudentViewModel> FindAsync(int Id);
        void Insert(StudentViewModel student);
        Task InsertAsync(StudentViewModel student);
        void Update(StudentViewModel student);
        void Delete(StudentViewModel student);
        void Delete(int Id);
        //Custom
        int Count(int Id);
        IQueryable<StudentViewModel> GetAll();
        StudentViewModel GetSingleByCondition(int Id);
        IQueryable<StudentViewModel> GetMulti();
        IQueryable<StudentViewModel> GetMultiPaging(int page, int pageSize, out int totalRow);
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

        public void Delete(StudentViewModel student)
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

        public StudentViewModel Find(int Id)
        {
            var query = _studentRepository.Find(Id);
            var queryModel = Mapper.Map<Student, StudentViewModel>(query);
            return queryModel;
        }

        public async Task<StudentViewModel> FindAsync(int Id)
        {
            var query = await _studentRepository.FindAsync(Id);
            var queryModel = Mapper.Map<Student, StudentViewModel>(query);
            return queryModel;
        }

        public IQueryable<StudentViewModel> FromSql(string sql, string Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentViewModel> GetAll()
        {
            //include for <<new string[] {"Enrollments"}>>
            var query = _studentRepository.GetAll(null);
            var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query);
            return queryModel.AsQueryable();
        }

        public IQueryable<StudentViewModel> GetMulti()
        {
            var query = _studentRepository.GetMulti(x => true);
            var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query);
            return queryModel.AsQueryable();
        }

        public IQueryable<StudentViewModel> GetMultiPaging(int page, int pageSize, out int totalRow)
        {
            var query = _studentRepository.GetMultiPaging(null, out totalRow, page, pageSize);
            var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query);
            return queryModel.AsQueryable();
        }

        public IQueryable<StudentViewModel> GetPagedList(string search, int pageindex, int pageSize)
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
            var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query);
            return queryModel.AsQueryable();
        }

        public async Task<IQueryable<StudentViewModel>> GetPagedListAsync(string search, int pageIndex, int pageSize)
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
            var queryModel = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(query.Items);
            return queryModel.AsQueryable();
        }

        public StudentViewModel GetSingleByCondition(int Id)
        {
            var query = _studentRepository.GetSingleByCondition(x => x.Id.Equals(Id));
            var queryModel = Mapper.Map<Student, StudentViewModel>(query);
            return queryModel;
        }

        public void Insert(StudentViewModel studentVM)
        {
            var newStudent = new Student();
            newStudent.UpdateStudent(studentVM);
            _studentRepository.Insert(newStudent);
        }

        public async Task InsertAsync(StudentViewModel studentVM)
        {
            var newStudent = new Student();
            newStudent.UpdateStudent(studentVM);
            await _studentRepository.InsertAsync(newStudent);
        }

        public void Update(StudentViewModel student)
        {
            var query = _studentRepository.Find(student.Id);
            query.UpdateStudent(student);
            _studentRepository.Update(query);
        }
    }
}
