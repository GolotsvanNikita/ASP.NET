namespace GuestBook.Services
{
    public interface IPasswordHash
    {
        string Hash(string password);
    }
}
