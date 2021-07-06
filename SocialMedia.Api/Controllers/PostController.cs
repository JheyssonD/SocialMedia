using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Net;
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetPosts([FromQuery] PostQueryFilter filters)
        {
            PagedList<Post> posts = PostService.GetPosts(filters);
            IEnumerable<PostDTO> postsDTO = Mapper.Map<IEnumerable<PostDTO>>(posts);
            ApiResponse<IEnumerable<PostDTO>> response = new ApiResponse<IEnumerable<PostDTO>>(postsDTO);

            response.Meta = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasPreviousPage = posts.HasPreviousPage,
                HasNextPage = posts.HasNextPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(response.Meta));
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPosts(int id)
        {
            Post post = await PostService.GetPost(id);
            PostDTO postDTO = Mapper.Map<Post, PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(PostDTO postDTO)
        {
            postDTO.Date = DateTime.Now;
            Post post = Mapper.Map<Post>(postDTO);
            await PostService.InsertPost(post);
            postDTO = Mapper.Map<PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put(int id, PostDTO postDTO)
        {
            Post post = Mapper.Map<Post>(postDTO);
            post.Id = id;
            await PostService.UpdatePost(post);
            postDTO = Mapper.Map<PostDTO>(post);
            ApiResponse<PostDTO> response = new ApiResponse<PostDTO>(postDTO);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            await PostService.DeletePost(id);
            ApiResponse<bool> response = new ApiResponse<bool>(true);
            return Ok(response);
        }
    }
}
