using Data.EntiyRepository;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface ICourseRepository : IEntityRepository<Course>
    {

    }
    public class CourseRepository : EntityRepository<Course> , ICourseRepository
    {
        public CourseRepository(HauLeDbContext entityDbContext) : base(entityDbContext)
        {

        }
    }
}
