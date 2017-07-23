using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class PaginationSet<T>
    {
        public int Page { set; get; }

        public int Count
        {
            get
            {
                return (Items != null) ? Items.Count() : 0;
            }

        }

        public int TotalPage { set; get; }
        public int TotalRow { set; get; }
        public int MaxPage { set; get; }
        public IQueryable<T> Items { set; get; }
    }
}
