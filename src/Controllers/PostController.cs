using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Dtos.Post;
using Catedra3Backend.src.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catedra3Backend.src.Controllers
{
    [Authorize]
    [Route("api/Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        [HttpPost("NewPost")]
        public async Task<IActionResult> Post([FromForm]UploadPostDto uploadPostDto)
        {
            try {
                var newPost = await _postRepository.CreateNewPost(uploadPostDto);

                var uri = Url.Action("GetPost", new { id = newPost!.PostId });
                var response = new
                {
                    Message = "Post created successfully",
                    Post = uploadPostDto
                };
                return Created(uri, response);
            } catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var posts = await _postRepository.GetAllPost();

                var response = new
                {
                    Message = "Products obtained succefully",
                    Post = posts
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }
    }
}