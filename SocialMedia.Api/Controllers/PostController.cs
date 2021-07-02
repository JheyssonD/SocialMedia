using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService PostService;
        private readonly IMapper Mapper;

        public PostController(IPostService postRepository, IMapper mapper)
        {
            PostService = postRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            IEnumerable<Post> posts = await PostService.GetPosts();
            IEnumerable<PostDTO> postsDTO = Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
            ApiResponse<IEnumerable<PostDTO>> response = new ApiResponse<IEnumerable<PostDTO>>(postsDTO);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosts(int id)
        {
            Post post = await PostService.GetPost(id);
            PostDTO postDTO = Mapper.Map<Post, PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDTO postDTO)
        {
            postDTO.Date = DateTime.Now;
            Post post = Mapper.Map<PostDTO, Post>(postDTO);
            await PostService.InsertPost(post);
            postDTO = Mapper.Map<Post, PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDTO postDTO)
        {
            Post post = Mapper.Map<PostDTO, Post>(postDTO);
            post.Id = id;
            await PostService.UpdatePost(post);
            postDTO = Mapper.Map<Post, PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await PostService.DeletePost(id);
            ApiResponse<bool> response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
