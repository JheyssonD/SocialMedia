using SocialMedia.Core.Entities;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUser(Security security);
    }
}