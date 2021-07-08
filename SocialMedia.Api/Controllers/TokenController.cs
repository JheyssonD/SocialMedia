using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public readonly IConfiguration Configuration;

        public TokenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin user)
        {
            //If is a valid user
            if (IsValidUser(user))
            {
                var token = generateToken(user);
                return Ok(new { token });
            }
            return NotFound();
        }

        private bool IsValidUser(UserLogin user)
        {
            return true;
        }

        private string generateToken(UserLogin user)
        {
            //header
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtHeader header = new JwtHeader(signingCredentials);

            //claims
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, "Jheysson Diaz"),
                new Claim(ClaimTypes.Email, "jjdg21090@gmail.com"),
                new Claim(ClaimTypes.Role, "Administrador"),
            };

            //payloads
            JwtPayload payload = new JwtPayload(
                Configuration["Authentication:Issuer"], 
                Configuration["Authentication:Audience"], 
                claims, DateTime.Now, 
                DateTime.UtcNow.AddMinutes(5)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
