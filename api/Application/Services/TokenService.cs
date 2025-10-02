using Application.Configurations;
using Application.Interface;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Services
{
    public class TokenService : IToken
    {
        private readonly JwtConfig _jwtConfig;
        public TokenService(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        public string CriarToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", usuario.Id.ToString()),
                new Claim("Cargo", usuario.Cargo.ToString()),
                new Claim("Email", usuario.Email),
                new Claim("Username", usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Cargo.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtConfig.Token));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(_jwtConfig.ExpiracaoHoras),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}