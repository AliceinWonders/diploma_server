using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using diploma_server.Account;
using Microsoft.IdentityModel.Tokens;

namespace diploma_server.Services;

public static class JwtTokenGenerator
{
    public static string GenerateJwtToken(User user, IConfiguration configuration)
    {
        // Логика генерации JWT-токена
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpiryMinutes"])),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}