namespace MusicPortal.DAL.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FilePath { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public int GenreId { get; set; }
        public virtual Genre? Genre { get; set; }
    }
}
