﻿namespace API.Filter
{
    public class PaginationFilter
    {
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 3;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 3 ? 3 : pageSize;
        }
    }
}