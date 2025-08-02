using AuthenServices.Data;
using AuthenServices.DTOs;
using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenServices.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<bool> UserExists(string username)
            => await _db.Users.AnyAsync(u => u.MaNV == username.ToLower());

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public string GenerateToken(Users user , List<string> domains, EmployeeInfoDto empInfo)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim("MaNV", user.MaNV ?? ""),
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim("PhongBan", empInfo.phongban ?? ""),
                new Claim("TinhTrangLamViec", empInfo.tinhtranglamviec ?? ""),
                new Claim("TenKip", empInfo.tenkip),
                new Claim("ToLamViec", empInfo.tolamviec),
                new Claim("PhanXuong", empInfo.phanxuong),
            };
            foreach (var domain in domains)
            {
                claims.Add(new Claim("Domain", domain));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
