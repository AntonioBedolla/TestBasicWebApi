using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestBasicWebApi.Services
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration config)
        {
            _secret = config["Jwt:Key"];
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
        }

        public string GenerateToken(int userId, string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username), // este el correo o nombre de usuario
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // Aquí guardamos el ID del usuario
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())// Id de token
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
