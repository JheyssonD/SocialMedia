using System;

namespace SocialMedia.Core.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserID{ get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
