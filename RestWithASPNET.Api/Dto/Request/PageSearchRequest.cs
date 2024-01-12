namespace RestWithASPNET.Api.Dto.Request
{
    public class PageSearchRequest<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
        public string SortFields { get; set; }
        public string SortDirection { get; set; }

        public Dictionary<string, Object> Filters { get; set; }

        public List<T> List { get; set; }

        public PageSearchRequest()
        {
        }

        public PageSearchRequest(int currentPage, int pageSize, string sortFields, string sortDirection)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortFields = sortFields;
            SortDirection = sortDirection;
        }

        public PageSearchRequest(int currentPage, int pageSize, string sortFields, string sortDirection, Dictionary<string, object> filters)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortFields = sortFields;
            SortDirection = sortDirection;
            Filters = filters;
        }

        public PageSearchRequest(int currentPage, string sortFields, string sortDirection) : this(currentPage, 10, sortFields, sortDirection)
        { }

        public int GetCurrentPage()
        {
            return CurrentPage == 0 ? 2 : CurrentPage;
        }

        public int GetPageSize()
        {
            return PageSize == 0 ? 10 : PageSize;
        }
    }
}