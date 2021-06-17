using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia.Infrastucture.Repositories
{
    public class PostRepository
    {
        public IEnumerable<Post> GetPosts()
        {
            var post = Enumerable.Range(1, 10).Select(i => new Post
            {
                PostId = i,
                Description = $"Descripcion {i}",
                Date = DateTime.Now,
                Image = $"https://misapis.com/{i}",
                UserID = i * 2
            });
            return post;
        }
    }
}
