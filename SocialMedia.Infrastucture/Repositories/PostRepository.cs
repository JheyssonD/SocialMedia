using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext Context;

        public PostRepository(SocialMediaContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            List<Post> posts = await Context.Posts.ToListAsync();
            return posts;
        }

        public async Task<Post> GetPost(int id)
        {
            Post post = await Context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            return post;
        }

        public async Task InsertPost(Post post)
        {
            Context.Posts.Add(post);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post modifiedPost)
        {
            Post currentPost = await GetPost(modifiedPost.PostId);
            currentPost.Comments = modifiedPost.Comments;
            currentPost.Date = modifiedPost.Date;
            currentPost.Image = modifiedPost.Image;
            int rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePost(int id)
        {
            Post currentPost = await GetPost(id);
            if (currentPost == null)
            {
                return false;
            }
            Context.Posts.Remove(currentPost);
            int rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
