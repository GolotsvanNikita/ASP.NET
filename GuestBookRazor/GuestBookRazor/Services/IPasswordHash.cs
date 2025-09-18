namespace GuestBookRazor.Services
{
    public interface IPasswordHash
    {
        string Hash(string password);
    }
}
