using Data.Infrastructure.UnitOfWork;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    interface IFunctionRepository: IRepository<Function>
    {

    }

    public class FunctionRepository : Repository<Function>, IFunctionRepository
    {
        public FunctionRepository(HauLeDbContext dbContext) : base(dbContext)
        {

        }
    }
}
