using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.BaseInterface
{
    public interface IBaseInterface<T> where T : class
    {

        void Insert(T t);

        void Update(T t);

        void Delete(int Id);

        IQueryable<T> GetAllPage(string keyword, int page, int pageSize, string sort, out int totalRow);

        IQueryable<T> GetAll();

        IQueryable<T> GetById(int Id);
    }
}
