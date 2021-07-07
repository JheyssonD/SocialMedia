using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
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
        private readonly PaginationOptions PaginationOptions;

        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options) 
        {
            UnitOfWork = unitOfWork;
            PaginationOptions = options.Value;
        }

        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? PaginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? PaginationOptions.DefaultPageSize : filters.PageSize;

            var posts = UnitOfWork.PostRepository.GetAll();

            if (filters.UserId != null)
            {
                posts = posts.Where(p => p.UserId == filters.UserId);
            }
            if (filters.Date != null)
            {
                posts = posts.Where(p => p.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }
            if (filters.Description != null)
            {
                posts = posts.Where(p => p.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            PagedList<Post> pagedPost = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);

            return pagedPost;
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
