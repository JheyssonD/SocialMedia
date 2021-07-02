using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> PostRepository;
        private readonly IRepository<User> UserRepository;

        public PostService(IRepository<Post> postRepository, IRepository<User> userRepository) 
        {
            PostRepository = postRepository;
            UserRepository = userRepository;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await PostRepository.GetAll();
        }

        public async Task<Post> GetPost(int id)
        {
           return await PostRepository.GetById(id);
        }

        public async Task InsertPost(Post post)
        {
            User user = await UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("Content not allowed");
            }
            await PostRepository.Add(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            return await PostRepository.Update(post);
        }

        public async Task<bool> DeletePost(int id)
        {
            return await PostRepository.Delete(id);
        }
    }
}
