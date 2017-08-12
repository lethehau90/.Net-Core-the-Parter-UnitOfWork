using Data.Infrastructure.UnitOfWork;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface IAppRoleRepository : IRepository<AppRole>
    {

    }
    public class AppRoleRepository : Repository<AppRole>, IAppRoleRepository
    {
        public AppRoleRepository(HauLeDbContext dbContext): base(dbContext)
        {

        }
    }
}
