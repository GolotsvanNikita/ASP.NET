namespace MusicPortal.Models
{
    public class SortViewModel
    {
        public SortState Current { get; set; }
        public bool Up { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            Current = sortOrder;
            Up = sortOrder == SortState.NameAsc || sortOrder == SortState.DurationAsc;
        }
    }
}