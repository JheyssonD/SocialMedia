namespace SocialMedia.Infrastucture.Interfaces
{
    public interface IPasswordHasher
    {
        string hash(string password);
        bool check(string hash, string password);
    }
}
