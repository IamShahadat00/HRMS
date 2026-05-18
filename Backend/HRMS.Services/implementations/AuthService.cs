using BCrypt.Net;
using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using HRMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.userName == dto.UserName);
            if (user == null)
            {
                return "User not found";
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PassWord))
            {
                return "Invalid password";
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.userName!),
                new Claim(ClaimTypes.Role, user.Role!),
                new Claim("UserId", user.Id.ToString()),
            };
                if (user.EmployeeId.HasValue)
                {
                    claims.Add(new Claim("EmployeeId", user.EmployeeId.Value.ToString()));
                }
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _config["Jwt:Issuer"],
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

            }
        }

        public async Task<string> RegisterAsync(UserRegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                return "Passwords do not match";
            }

            var exists = await _context.Users.AnyAsync(u => u.userName == dto.UserName);
            if (exists)
            {
                return "Username already taken";
            }

            var user = new User
            {
                userName = dto.UserName,
                PassWord = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role ?? "Employee"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "Registration successful";
        }
    }
}