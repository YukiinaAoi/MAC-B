using Microsoft.AspNetCore.Mvc;
using QRAppAPI.Data;
using QRAppAPI.Models;
using BCrypt.Net;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QRAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly QRAppDbContext _context;

        public AuthController(QRAppDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/signup
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User newUser)
        {
            if (newUser == null || string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.PasswordHash))
            {
                return BadRequest("Invalid user data.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            // Hash the password
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);

            // Add the user to the database
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User created successfully!" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Invalid login data.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                IDNumber = user.IDNumber,
                ImageData = user.ImageData,
                IsAdmin = user.IsAdmin
            });
        }
    }
}
