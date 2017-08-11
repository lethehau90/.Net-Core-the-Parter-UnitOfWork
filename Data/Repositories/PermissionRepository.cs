using Data.Infrastructure.UnitOfWork;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    interface IPermissionRepository: IRepository<Permission>
    {

    }
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(HauLeDbContext dbContext): base(dbContext)
        {

        }
    }
}
