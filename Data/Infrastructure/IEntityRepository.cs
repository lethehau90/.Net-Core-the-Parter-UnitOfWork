using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.EntiyRepository
{
    public interface IEntityRepository<T> where T : class, new()
    {

        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        PaginatedList<T> Paginate<TKey>(
            int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector);

        PaginatedList<T> Paginate<TKey>(
            int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Edit(T entity);

        void Delete(T entity);

        void UpdateRange(List<T> entities);

        void InsertRange(List<T> entities, int batchSize = 100);

        void DeleteRange(List<T> entities);

        IQueryable<T> GetMany(Expression<Func<T, bool>> where, string includes);
        int Count(Expression<Func<T, bool>> where);
        IQueryable<T> GetAll(string[] includes = null);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);
        IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);
        IQueryable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 20, string[] includes = null);

        bool CheckContains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// SUBMIT CHANGES dataContext
        /// </summary>
        void Save();


    }
}