using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository PostRepository;
        private readonly IUserRepository UserRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository) 
        {
            PostRepository = postRepository;
            UserRepository = userRepository;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await PostRepository.GetPosts();
        }

        public async Task<Post> GetPost(int id)
        {
           return await PostRepository.GetPost(id);
        }

        public async Task InsertPost(Post post)
        {
            User user = await UserRepository.GetUser(post.UserId);
            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("Content not allowed");
            }
            await PostRepository.InsertPost(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            return await PostRepository.UpdatePost(post);
        }

        public async Task<bool> DeletePost(int id)
        {
            return await PostRepository.DeletePost(id);
        }
    }
}
