using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork UnitOfWork;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<Security> GetLoginByUser(UserLogin userLogin)
        {
            return await UnitOfWork.SecurityRepository.GetLoginByUser(userLogin);
        }

        public async Task RegisterUser(Security security)
        {
            await UnitOfWork.SecurityRepository.Add(security);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
