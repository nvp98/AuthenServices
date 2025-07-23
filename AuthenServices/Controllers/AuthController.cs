using AuthenServices.Data;
using AuthenServices.DTOs;
using AuthenServices.Models;
using AuthenServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly AuthService _auth;

        public AuthController(AppDbContext db, AuthService auth)
        {
            _db = db;
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.MaNV == dto.Username.ToLower());
            if (user == null || !_auth.VerifyPassword(dto.Password, user.MatKhau))
                return Unauthorized("Invalid credentials");

            var token = _auth.GenerateToken(user);
            return Ok(new { token });
        }
        [HttpPost("hashPassword")]
        public async Task<IActionResult> hashPassword(string matkhau)
        {
            string hash = _auth.HashPassword(matkhau);
            return Ok(new { hash });
        }
    }
}
