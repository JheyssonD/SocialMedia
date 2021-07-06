using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia.Core.CustomEntities
{
    public class PagedList<T> : List<T>
    {
        private int pageSize;
        private int currentPage;

        public int CurrentPage { get => currentPage; set => currentPage = (value > 0) ? value : 1; }
        public int TotalPages { get; set; }
        public int PageSize { get => pageSize; set => pageSize = (value > 0) ? value : 9999; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : (int?)null;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : (int?)null;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(items);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            List<T> items = source.ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize).Paginate();
        }

        public PagedList<T> Paginate()
        {
            List<T> items = this.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            Clear();
            AddRange(items);
            return this;
        }

    }
}
