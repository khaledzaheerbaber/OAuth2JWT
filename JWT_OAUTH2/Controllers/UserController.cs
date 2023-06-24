using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT_OAUTH2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserController( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {
            
            if (user != null && user.Username == "admin" && user.Password == "admin")
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "claim1"),
                };
                authClaims.Add(new Claim(ClaimTypes.Role, "adminrole"));
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}
