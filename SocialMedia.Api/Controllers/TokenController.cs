using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public readonly IConfiguration Configuration;
        public readonly ISecurityService SecurityService;

        public TokenController(IConfiguration configuration, ISecurityService securityService)
        {
            Configuration = configuration;
            SecurityService = securityService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticationAsync(UserLogin user)
        {
            //If is a valid user
            (bool, Security) validation = await IsValidUserAsync(user);
            if (validation.Item1)
            {
                var token = generateToken(validation.Item2);
                return Ok(new { token });
            }
            return NotFound();
        }

        private async Task<(bool, Security)> IsValidUserAsync(UserLogin login)
        {
            Security user = await SecurityService.GetLoginByCredentials(login);
            return (user != null, user);
        }

        private string generateToken(Security user)
        {
            //header
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtHeader header = new JwtHeader(signingCredentials);

            //claims
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("User", user.User),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            //payloads
            JwtPayload payload = new JwtPayload(
                Configuration["Authentication:Issuer"], 
                Configuration["Authentication:Audience"], 
                claims, DateTime.Now, 
                DateTime.UtcNow.AddMinutes(Convert.ToDouble(Configuration["Authentication:Expiration"]))
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
