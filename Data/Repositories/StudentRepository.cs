using Common.Store;
using Data.EntiyRepository;
using Data.Infrastructure;
using Data.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        IQueryable<StudentStore> getModelFromQuery(string Id);
    }
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Student> _dbSet;
        public StudentRepository(HauLeDbContext entityDbContext) : base(entityDbContext)
        {
            _dbContext = entityDbContext ?? throw new ArgumentNullException(nameof(entityDbContext));
            _dbSet = _dbContext.Set<Student>(); 

        }
        public IQueryable<StudentStore> getModelFromQuery(string Id)
        {
            SqlParameter[] queryParams = new SqlParameter[] { new SqlParameter("@Id", Id) };
            var result = RDFacadeExtensions.GetModelFromQuery<StudentStore>(_dbContext, "getById @Id", queryParams).AsQueryable();
            return result;
        }
       
    }
}
