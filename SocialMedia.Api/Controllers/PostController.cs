using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository PostRepository;
        private readonly IMapper Mapper;

        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            PostRepository = postRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            IEnumerable<Post> posts = await PostRepository.GetPosts();
            IEnumerable<PostDTO> postsDTO = Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
            return Ok(postsDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosts(int id)
        {
            Post post = await PostRepository.GetPost(id);
            PostDTO postDTO = Mapper.Map<Post, PostDTO>(post);
            return Ok(postDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDTO postDTO)
        {
            postDTO.Date = DateTime.Now;
            Post post = Mapper.Map<PostDTO, Post>(postDTO);

            await PostRepository.InsertPost(post);
            return Ok(post);
        }
    }
}
