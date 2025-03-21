using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PaginationResult
    {
        public ICollection<object> Content { get; set; } = new List<object>();

        public int ItemAmount { get; set; }

        public int PageSize { get; set; }

        public int PageCount => (int) (((float) ItemAmount / PageSize) + 0.5);

        public int CurrentPage { get; set; }
    }

    public class PaginationResult<T>: PaginationResult
    {
        new public ICollection<T> Content { get; set; } = new List<T>();
    }
}
