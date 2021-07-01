using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public interface IPostService
    {
        public Task<IEnumerable<Post>> GetPosts();
        public Task<Post> GetPost(int id);
        public Task InsertPost(Post post);
        public Task<bool> UpdatePost(Post post);
        public Task<bool> DeletePost(int id);
    }
}