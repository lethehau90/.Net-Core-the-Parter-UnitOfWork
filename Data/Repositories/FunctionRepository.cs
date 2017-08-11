using Data.Infrastructure.UnitOfWork;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public interface IFunctionRepository : IRepository<Function>
    {
        IQueryable<Function> GetListFunctionWithPermission(string userId);
    }

    public class FunctionRepository : Repository<Function>, IFunctionRepository
    {
        private readonly HauLeDbContext _dbContext;
        public FunctionRepository(HauLeDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Function> GetListFunctionWithPermission(string userId)
        {

            var query = (from T1 in _dbContext.Functions
                         join T2 in _dbContext.Permissions on T1.ID equals T2.FunctionId
                         join T3 in _dbContext.AppRoles on T2.RoleId equals T3.Id
                         join T4 in _dbContext.UserRoles on T3.Id equals T4.RoleId
                         join T5 in _dbContext.Users on T4.UserId equals T5.Id
                         where T5.Id == userId && (T2.CanRead == true)
                         select T1);
            var parentIds = query.Select(x => x.ParentId).Distinct();
            query = query.Union(_dbContext.Functions.Where(f => parentIds.Contains(f.ID)));
            return query;
        }
    }
}
