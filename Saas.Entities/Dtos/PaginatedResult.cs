using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saas.Entities.Dtos
{
    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PaginatedResult(List<T> data, int totalItems, int currentPage, int pageSize)
        {
            Data = data;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }

}