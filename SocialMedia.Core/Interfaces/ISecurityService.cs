using SocialMedia.Core.Entities;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByUser(UserLogin userLogin);
        Task RegisterUser(Security security);
    }
}