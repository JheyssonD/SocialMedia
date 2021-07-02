using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork  UnitOfWork;

        public PostService(IUnitOfWork unitOfWork) 
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await UnitOfWork.PostRepository.GetAll();
        }

        public async Task<Post> GetPost(int id)
        {
           return await UnitOfWork.PostRepository.GetById(id);
        }

        public async Task InsertPost(Post post)
        {
            User user = await UnitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("Content not allowed");
            }
            await UnitOfWork.PostRepository.Add(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            return await UnitOfWork.PostRepository.Update(post);
        }

        public async Task<bool> DeletePost(int id)
        {
            return await UnitOfWork.PostRepository.Delete(id);
        }
    }
}
