using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using NoteApi.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;

namespace NoteApi.Services
{
    public class UserRepository(DbContext dbContext, IConfiguration configuration)
    {
        public async Task<UserResponse<UserApiResponse?>> GetUser(string username, string password)
        {
            using var connection = dbContext.CreateConnection();

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM dbo.Users WHERE Username = @Username",
                new { Username = username }
            );

            if (user == null)
                return new UserResponse<UserApiResponse?> { Data = null, Token = null };

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);


            try
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    // Password incorrect
                    return new UserResponse<UserApiResponse?>
                    {
                        Data = null,
                        Token = null
                    };
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                throw;
            }


            var token = GenerateJwtToken(user.Id, user.Username);

            var res = new UserResponse<UserApiResponse?>
            {
                Data = new UserApiResponse
                {
                    Id = user.Id,
                    Username = username,
                    Role = user.Role,
                },
                Token = token
            };

            return res;
        }

        private string GenerateJwtToken(int userId, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("Username", username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Audience"],
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpireMinutes"])),
               signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
