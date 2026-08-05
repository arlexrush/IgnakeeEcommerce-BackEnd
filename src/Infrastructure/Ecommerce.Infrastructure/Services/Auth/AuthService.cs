using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Models.Token;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {

        public JwtSettings _jwtSettings { get; }

        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings.Value;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string CreateToken(User user, IList<string>? roles)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName!),
                new Claim("userId", user.Id),
                new Claim("email", user.Email!)};

            foreach (var rol in roles!)
            {
                var claim = new Claim(ClaimTypes.Role, rol);
                claims.Add(claim);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var tokenResponse = tokenHandler.WriteToken(token);
            return tokenResponse;

        }

        public string GetSessionUser()
        {
            var claims = httpContextAccessor.HttpContext?.User?.Claims;
            var userName = claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                ?? claims?.FirstOrDefault(x => x.Type == "preferred_username")?.Value
                ?? claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email || x.Type == "email")?.Value;

            return userName ?? throw new InvalidOperationException("The authenticated user does not contain a usable profile identifier.");
        }
    }
}
