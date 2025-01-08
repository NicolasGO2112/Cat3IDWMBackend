using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catedra3Backend.src.Dtos.Auth;
using Catedra3Backend.src.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catedra3Backend.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private static readonly List<string> RevokedTokens = new List<string>();
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var auth = await _authRepository.RegisterUserAsync(registerDto);
                return Ok(auth);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPost("Loggin")]
        public async Task<IActionResult> Login([FromBody] LogginDto logginDto)
        {
            try
            {
                var auth = await _authRepository.LogginUserAsync(logginDto);
                return Ok(auth);
            }
            catch (Exception e)
            {
                if (e.Message == "Invalid email or password")
                {
                    return Unauthorized(new { message = e.Message });
                }
                return BadRequest(new { message = e.Message });
            }
        }
    }
}