using HotelTransilvania.Models;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelTransilvania.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usuario usuario);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new Claim[]
            {
                new Claim("Id", "24"),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, "SOME_EMAIL"),
                new Claim(JwtRegisteredClaimNames.Name,"SOME_NAME")
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                SecurityAlgorithms.HmacSha256
            );

            var expirationDate = DateTime.Now.AddYears(1000);

            var securityToken = new JwtSecurityToken(
                            jwt.Issuer,
                            jwt.Audience,
                            claims,
                            null,
                            expirationDate,
                            signingCredentials
                        );


            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
            //var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
            //    new Claim("Id", usuario.Id.ToString()),
            //    new Claim(ClaimTypes.Role, usuario.Rol)
            //};

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Console.WriteLine(signIn);
            //var token = new JwtSecurityToken(
            //    jwt.Issuer,
            //    jwt.Audience,
            //    claims,
            //    expires: DateTime.Now.AddMonths(1),
            //    signingCredentials: signIn
            //    );

            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
