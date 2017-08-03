using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Data.Infrastructure
{
    public static class RDFacadeExtensions
    {
        public static RelationalDataReader ExecuteSqlQuery(
            this DatabaseFacade databaseFacade,
            string sql,
            SqlParameter[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();
            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                return rawSqlCommand
                    .RelationalCommand
                    .ExecuteReader(
                        databaseFacade.GetService<IRelationalConnection>(),
                        parameterValues: rawSqlCommand.ParameterValues);
            }
        }

        public static IEnumerable<T> GetModelFromQuery<T>(
            DbContext context,
            string sql,
            SqlParameter[] parameters)
            where T : new()
        {
            DatabaseFacade databaseFacade = new DatabaseFacade(context);
            using (DbDataReader dr = databaseFacade.ExecuteSqlQuery(sql, parameters).DbDataReader)
            {
                List<T> lst = new List<T>();
                PropertyInfo[] props = typeof(T).GetProperties();
                while (dr.Read())
                {
                    T t = new T();
                    IEnumerable<string> actualNames = dr.GetColumnSchema().Select(o => o.ColumnName);
                    for (int i = 0; i < props.Length; ++i)
                    {
                        PropertyInfo pi = props[i];
                        if (!pi.CanWrite) continue;
                        System.ComponentModel.DataAnnotations.Schema.ColumnAttribute ca = pi.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)) as System.ComponentModel.DataAnnotations.Schema.ColumnAttribute;
                        string name = ca?.Name ?? pi.Name;
                        if (pi == null) continue;
                        if (!actualNames.Contains(name)) { continue; }
                        object value = dr[name];
                        Type pt = pi.DeclaringType;
                        bool nullable = pt.GetTypeInfo().IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>);
                        if (value == DBNull.Value) { value = null; }
                        if (value == null && pt.GetTypeInfo().IsValueType && !nullable)
                        { value = Activator.CreateInstance(pt); }
                        pi.SetValue(t, value);
                    }//for i
                    lst.Add(t);
                }//while
                return lst;
            }//using dr
        }
    }
}
