using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Entities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Repositories;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository PostRepository;

        public PostController(IPostRepository postRepository)
        {
            PostRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            IEnumerable<Post> posts = await PostRepository.GetPosts();
            IEnumerable<PostDTO> postDTO = posts.Select(x => new PostDTO
            {
                PostId = x.PostId,
                Date = x.Date,
                Description = x.Description,
                Image = x.Image,
                UserId = x.UserId
            });
            return Ok(postDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosts(int id)
        {
            Post post = await PostRepository.GetPost(id);
            PostDTO postDTO = new PostDTO
            {
                PostId = post.PostId,
                Date = post.Date,
                Description = post.Description,
                Image = post.Image,
                UserId = post.UserId
            };
            return Ok(postDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDTO postDTO)
        {
            postDTO.Date = DateTime.Now;
            Post post = new Post
            {
                PostId = postDTO.PostId,
                Date = postDTO.Date,
                Description = postDTO.Description,
                Image = postDTO.Image,
                UserId = postDTO.UserId
            };
            await PostRepository.InsertPost(post);
            return Ok(post);
        }
    }
}
