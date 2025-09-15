namespace MusicPortal.Controllers
{
    public class PageViewModel
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public PageViewModel(int totalItems, int currentPage, int pageSize)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
