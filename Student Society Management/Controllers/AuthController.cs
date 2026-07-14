using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Society_Management.Data;
using Student_Society_Management.Models;

namespace Student_Society_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SocietyDbContext _context;
        public AuthController(SocietyDbContext context) => _context = context;

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public int UserId { get; set; }
            public string Role { get; set; } = string.Empty;
            public int? MemberId { get; set; }
            public string FullName { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password" });

            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == user.Id);

            return Ok(new LoginResponse
            {
                UserId = user.Id,
                Role = user.Role,
                MemberId = member?.Id,
                FullName = member?.FullName ?? user.Username
            });
        }

        public class RegisterRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string StudentNumber { get; set; } = string.Empty;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest(new { message = "Username already taken" });

            // Self-registration always creates a Member — Admin accounts are created
            // separately (via the seed method or directly by another admin), never
            // through this public endpoint.
            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Member"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var member = new Member
            {
                FullName = request.FullName,
                Email = request.Email,
                StudentNumber = request.StudentNumber,
                JoinDate = DateTime.Now,
                UserId = user.Id
            };
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account created — you can now sign in" });
        }

        [HttpPost("seed-test-users")]
        public async Task<IActionResult> SeedTestUsers()
        {
            _context.Users.Add(new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("adminpass"), Role = "Admin" });
            _context.Users.Add(new User { Username = "member1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("member123"), Role = "Member" });
            await _context.SaveChangesAsync();

            var memberUser = await _context.Users.FirstAsync(u => u.Username == "member1");
            _context.Members.Add(new Member { FullName = "Test Member", Email = "member1@example.com", StudentNumber = "20001111", JoinDate = DateTime.Now, UserId = memberUser.Id });
            await _context.SaveChangesAsync();

            return Ok("Seeded admin/admin123 and member1/member123");
        }
    }
}
