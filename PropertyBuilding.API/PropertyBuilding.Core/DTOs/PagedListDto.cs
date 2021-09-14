using PropertyBuilding.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertyBuilding.Core.DTOs
{
    public class PagedListDto<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPages => CurrentPage > 1;
        public bool HasNextPages => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPages ? CurrentPage + 1 : null;
        public int? PreviousPageNumber => HasPreviousPages ? CurrentPage - 1 : null;

        public PagedListDto(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PagedListDto<T> Pagination(IEnumerable<T> source, int pageNumber, int pageSize, PaginationOptions paginationOptions)
        {
            var count = source.Count();
            pageNumber = pageNumber == 0 ? paginationOptions.DefaultPageNumber : pageNumber;
            pageSize = pageSize == 0 ? paginationOptions.DefaultPageSize : pageSize;
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedListDto<T>(items, count, pageNumber, pageSize);
        }
    }
}
