using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
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
            ApiResponse<IEnumerable<PostDTO>> response = new ApiResponse<IEnumerable<PostDTO>>(postsDTO);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosts(int id)
        {
            Post post = await PostRepository.GetPost(id);
            PostDTO postDTO = Mapper.Map<Post, PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDTO postDTO)
        {
            postDTO.Date = DateTime.Now;
            Post post = Mapper.Map<PostDTO, Post>(postDTO);
            await PostRepository.InsertPost(post);
            postDTO = Mapper.Map<Post, PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDTO postDTO)
        {
            Post post = Mapper.Map<PostDTO, Post>(postDTO);
            post.PostId = id;
            await PostRepository.UpdatePost(post);
            postDTO = Mapper.Map<Post, PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await PostRepository.DeletePost(id);
            ApiResponse<bool> response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
