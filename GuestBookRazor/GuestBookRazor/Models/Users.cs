namespace GuestBookRazor.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }

        public string? Password { get; set; }

        public virtual IList<Message>? Messages { get; set; }
    }
}