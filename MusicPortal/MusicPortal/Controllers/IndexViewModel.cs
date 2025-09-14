using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Entities;
using MusicPortal.Models;

namespace MusicPortal.Controllers
{
    public class IndexViewModel
    {
        public IEnumerable<SongDTO> Songs { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; } = new SortViewModel(SortState.NameAsc);
        public string SearchString { get; set; }
        public int? GenreId { get; }

        public IndexViewModel(IEnumerable<SongDTO> songs, PageViewModel pageViewModel, SortViewModel sortViewModel, string searchString, int? genreId = null)
        {
            Songs = songs;
            PageViewModel = pageViewModel;
            SortViewModel = sortViewModel;
            SearchString = searchString;
            GenreId = genreId;
        }
    }
}
