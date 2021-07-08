using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.Services;
using SocialMedia.Infrastucture.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService PostService;
        private readonly IMapper Mapper;
        private readonly IUriService UriService;

        public PostController(IPostService postRepository, IMapper mapper, IUriService uriService)
        {
            PostService = postRepository;
            Mapper = mapper;
            UriService = uriService;
        }

        /// <summary>
        /// Retrieve all Posts
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDTO>>))]
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
                HasNextPage = posts.HasNextPage,
                PreviousPageUrl = UriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
                NextPageUrl = UriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
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
