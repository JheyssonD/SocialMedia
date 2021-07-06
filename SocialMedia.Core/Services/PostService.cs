using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
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

        public IEnumerable<Post> GetPosts(PostQueryFilter filters)
        {
            var posts = UnitOfWork.PostRepository.GetAll();

            if (filters.userId != null)
            {
                posts = posts.Where(p => p.UserId == filters.userId);
            }
            if (filters.date != null)
            {
                posts = posts.Where(p => p.Date.ToShortDateString() == filters.date?.ToShortDateString());
            }
            if (filters.description != null)
            {
                posts = posts.Where(p => p.Description.ToLower().Contains(filters.description.ToLower()));
            }
            return posts;
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

        public async Task<bool> UpdatePost(Post post)
        {
            UnitOfWork.PostRepository.Update(post);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(int id)
        {
            await UnitOfWork.PostRepository.Delete(id);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
