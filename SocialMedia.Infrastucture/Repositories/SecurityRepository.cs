using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(SocialMediaContext context) : base(context) { }

        public async Task<Security> GetLoginByUser(UserLogin login)
        {
            return await Entities.FirstOrDefaultAsync(u => u.User == login.User);
        }
    }
}
