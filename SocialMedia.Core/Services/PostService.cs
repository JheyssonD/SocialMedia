using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Post> GetPosts()
        {
            return UnitOfWork.PostRepository.GetAll();
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
                throw new BusinessException("User doesn't exist");
            }
            IEnumerable<Post> UserPost = await UnitOfWork.PostRepository.GetByUser(post.UserId);
            if (UserPost.Count() > 10)
            {
                Post lastPost = UserPost.OrderBy(p => p.Date).LastOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("You are not able to publish the post");
                }
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new BusinessException("Content not allowed");
            }
            await UnitOfWork.PostRepository.Add(post);
            await UnitOfWork.SaveChangesAsync();
        }

        public void UpdatePost(Post post)
        {
            UnitOfWork.PostRepository.Update(post);
            UnitOfWork.SaveChanges();
        }

        public async Task DeletePost(int id)
        {
            await UnitOfWork.PostRepository.Delete(id);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
