using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Repositories;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetPost()
        {
            var post = await PostRepository.GetPosts();
            return Ok(post);
        }
    }
}
