using Data.EntiyRepository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IEnrollmentRepository : IEntityRepository<Enrollment>
    {

    }
    public class EnrollmentRepository : EntityRepository<Enrollment> , IEnrollmentRepository
    {
        public EnrollmentRepository(HauLeDbContext entityDbContext) : base(entityDbContext)
        {

        }
    }
}
