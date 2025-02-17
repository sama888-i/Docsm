using Docsm.Exceptions;
using Docsm.Helpers.Enums;
using Docsm.Models;
using Docsm.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Docsm.Helpers
{
    public class jwtTokens
    {
        private  readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;
        public jwtTokens(UserManager<User> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            List<Claim> claims =
                [
                new Claim(ClaimTypes.NameIdentifier , user.Id ),
                new Claim(ClaimTypes.Name ,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Gender ,user.Gender.ToString()),
                new Claim(ClaimTypes.DateOfBirth ,user.DateOfBirth.ToString("yyyy-MM-dd")),
               
                new Claim("FullName",user.Name+" "+user.Surname)

            ];

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey ));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSec = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer ,
                audience: _jwtOptions.Audience ,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred
                );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(jwtSec);
        }

    }
}
