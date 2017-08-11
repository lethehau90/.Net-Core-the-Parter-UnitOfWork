using Data.Infrastructure.UnitOfWork;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Data.Repositories
{
    public interface IPermissionRepository: IRepository<Permission>
    {
        IQueryable<Permission> GetByUserId(string userId);
    }
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        HauLeDbContext _dbContext;
        public PermissionRepository(HauLeDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Permission> GetByUserId(string userId)
        {
            var query = from T1 in _dbContext.Functions
                        join T2 in _dbContext.Permissions on T1.ID equals T2.FunctionId
                        join T3 in _dbContext.AppRoles on T2.RoleId equals T3.Id
                        join T4 in _dbContext.UserRoles on T3.Id equals T4.RoleId
                        join T5 in _dbContext.AppRoles on T4.UserId equals T5.Id
                        where T5.Id == userId
                        select T2;
            return query;
        }
    }
}
