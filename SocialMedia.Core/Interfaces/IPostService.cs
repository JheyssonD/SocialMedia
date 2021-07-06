using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public interface IPostService
    {
        public IEnumerable<Post> GetPosts(PostQueryFilter filters);
        public Task<Post> GetPost(int id);
        public Task InsertPost(Post post);
        public Task<bool> UpdatePost(Post post);
        public Task<bool> DeletePost(int id);
    }
}