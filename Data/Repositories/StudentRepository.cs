using Data.EntiyRepository;
using Data.Infrastructure.UnitOfWork;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {

    }
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(HauLeDbContext entityDbContext) : base(entityDbContext)
        {

        }
    }
}
