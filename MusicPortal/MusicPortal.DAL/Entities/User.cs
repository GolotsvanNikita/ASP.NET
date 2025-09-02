namespace MusicPortal.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string? PasswordHash { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Song>? Songs { get; set; }

        public User()
        {
            Songs = [];
        }
    }
}
