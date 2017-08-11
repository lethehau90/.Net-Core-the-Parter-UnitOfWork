using Data.Infrastructure.UnitOfWork;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public interface  IErrorRepository : IRepository<Error>
    {

    }
    public class ErrorRepository : Repository<Error>, IErrorRepository 
    {
        public ErrorRepository(HauLeDbContext entityDbcontext): base(entityDbcontext)
        {

        }
    }
}
