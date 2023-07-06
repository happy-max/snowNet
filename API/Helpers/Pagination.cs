using Core.Specifications;

namespace API.Helpers
{
    public class Pagination<T>where T : class
    {
        private int TotalItems;

        public Pagination(int page, int pageSize, int totalItems, IReadOnlyList<T> data)
        {
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            Data = data;
        }

        public int Page {get; set;}
        public int PageSize {get; set;}
        public int Count {get; set;}
        public IReadOnlyList<T> Data {get; set;}
    }
}