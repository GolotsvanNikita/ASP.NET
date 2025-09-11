using MusicPortal.BLL.DTO;

namespace MusicPortal.Controllers
{
    public class IndexViewModel
    {
        public IEnumerable<SongDTO> Songs { get; set; }
        public PageViewModel PageViewModel { get; set; }

        public IndexViewModel(IEnumerable<SongDTO> songs, PageViewModel pageViewModel)
        {
            Songs = songs;
            PageViewModel = pageViewModel;
        }
    }
}
