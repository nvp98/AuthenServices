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
        private readonly EmployeeService _employeeService;

        public AuthController(AppDbContext db, AuthService auth, EmployeeService employeeService)
        {
            _db = db;
            _auth = auth;
            _employeeService = employeeService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var username = dto.Username.Trim();

            var empInfo = await _employeeService.GetEmployeeInfoAsync(username);
            if (empInfo == null)
                return Forbid("Tài khoản không đủ điều kiện đăng nhập");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.MaNV == username);
            if (user == null || !_auth.VerifyPassword(dto.Password, user.MatKhau))
                return Unauthorized("Sai mật khẩu.");

            var userDomains = await (
                from ud in _db.UserDomains
                join d in _db.Domains on ud.DomainID equals d.ID
                where ud.UserID == user.ID && d.IsActive
                select d.DomainUrl
            ).ToListAsync();

            var token = _auth.GenerateToken(user, userDomains, empInfo);
            return Ok(new
            {
                token,
                message = " Đăng nhập thành công",
                hoten = empInfo.hoten,
                manv = empInfo.manv,
                phongban= empInfo.phongban,
                tinhtranglamviec= empInfo.tinhtranglamviec

            });
        }


        [HttpPost("hashPassword")]
        public async Task<IActionResult> hashPassword(string matkhau)
        {
            string hash = _auth.HashPassword(matkhau);
            return Ok(new { hash });
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(UserLoginDto dto)
        //{
        //    var user = await _db.Users.FirstOrDefaultAsync(u => u.MaNV == dto.Username.ToLower());
        //    if (user == null || !_auth.VerifyPassword(dto.Password, user.MatKhau))
        //        return Unauthorized("Invalid credentials");

        //    var token = _auth.GenerateToken(user);
        //    return Ok(new { token });
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var username = dto.Username.Trim();

            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.MaNV == username);
            if (existingUser != null)
            {
                return BadRequest("Tài khoản đã tồn tại.");
            }

            // Tạo user mới
            var hashedPassword = _auth.HashPassword(dto.Password);
            var newUser = new Users
            {
                MaNV = username,
                MatKhau = hashedPassword,
                HoTen = dto.HoTen,
                TinhTrangLV = 1,
                Email = dto.Email
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            return Ok("Đăng ký tài khoản thành công.");
        }
        

    }
}
