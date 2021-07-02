using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public interface IPostService
    {
        public IEnumerable<Post> GetPosts();
        public Task<Post> GetPost(int id);
        public Task InsertPost(Post post);
        public void UpdatePost(Post post);
        public Task DeletePost(int id);
    }
}