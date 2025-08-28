namespace GuestBook.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? MessageText { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
