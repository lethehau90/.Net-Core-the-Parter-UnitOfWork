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

namespace Service
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
        int Count();
        IQueryable<StudentViewModel> GetAll();
        StudentViewModel GetSingleByCondition(int Id);
        IQueryable<StudentViewModel> GetMulti(string name);
        IQueryable<StudentViewModel> GetMultiPaging(int page, int pageSize,out int totalRow);
        bool CheckContains();
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

        public bool CheckContains()
        {
            return _studentRepository.CheckContains(x=>x.Id.Equals(2));
        }

        public int Count()
        {
            return _studentRepository.Count(null);
        }

        public void Delete(StudentViewModel student)
        {
           if(student.Id != null && student.Id != 0)
            {
                _studentRepository.Delete(student);
            }
            
        }

        public void Delete(int Id)
        {
            _studentRepository.Delete(Id);
        }

        public StudentViewModel Find(int Id)
        {
            var query = _studentRepository.Find(Id);
            var queryModel =  Mapper.Map<Student, StudentViewModel>(query);
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
            var query = _studentRepository.GetAll(null,new string[] { "Enrollments" });
            var queryModel = Mapper.Map<IQueryable<Student>, IQueryable<StudentViewModel>>(query);
            return queryModel;
        }

        public IQueryable<StudentViewModel> GetMulti(string name)
        {
            var query = _studentRepository.GetMulti(x=>x.Name.Contains(name));
            var queryModel = Mapper.Map<IQueryable<Student>, IQueryable<StudentViewModel>>(query);
            return queryModel;
        }

        public IQueryable<StudentViewModel> GetMultiPaging(int page, int pageSize, out int totalRow)
        {
            var query = _studentRepository.GetMultiPaging(null,out totalRow,page,pageSize);
            var queryModel = Mapper.Map<IQueryable<Student>, IQueryable<StudentViewModel>>(query);
            return queryModel;
        }

        public IQueryable<StudentViewModel> GetPagedList(string search, int pageindex, int pageSize)
        {
            var query = _studentRepository.GetPagedList(null, null, pageindex, pageSize).Items;
            var queryModel = Mapper.Map<IEnumerable<Student>, IQueryable<StudentViewModel>>(query);
            return queryModel;
        }

        public async Task<IQueryable<StudentViewModel>>  GetPagedListAsync(string search, int pageIndex, int pageSize)
        {
            var query = await _studentRepository.GetPagedListAsync(x=>x.Name.Contains(search), null, null, pageIndex, pageSize);
            var queryModel = Mapper.Map<IEnumerable<Student>, IQueryable<StudentViewModel>>(query.Items);
            return queryModel;
        }

        public StudentViewModel GetSingleByCondition(int Id)
        {
            var query = _studentRepository.GetSingleByCondition(x => x.Id.Equals(Id));
            var queryModel = Mapper.Map<Student,StudentViewModel>(query);
            return queryModel;
        }

        public void Insert(StudentViewModel student)
        {
            var dataModel = Mapper.Map<StudentViewModel, Student>(student);
            _studentRepository.Insert(dataModel);
            _unitOfWork.SaveChanges();

        }

        public async Task InsertAsync(StudentViewModel student)
        {
            var dataModel = Mapper.Map<StudentViewModel, Student>(student);
            _studentRepository.Insert(dataModel);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Update(StudentViewModel student)
        {
            var dataModel = Mapper.Map<StudentViewModel, Student>(student);
            _studentRepository.Update(dataModel);
            _unitOfWork.SaveChanges();
        }
    }
}
