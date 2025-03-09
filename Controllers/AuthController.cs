using fitapp.Data;
using fitapp.DTOs;
using fitapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Globalization;
using fitapp.Services;

namespace fitapp.Controllers

{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FitnessAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthController(FitnessAppContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService; // ✅ قم بتهيئة _emailService هنا
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        }





        // private string GenerateJwtToken(User user)
        private string GenerateJwtToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user.Email), "User email cannot be null or empty");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentNullException("JWT key is missing"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim("UserID", user.UserId.ToString()),
            new Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? string.Empty)
        }),
                Expires = DateTime.UtcNow.AddHours(1), // تحديد صلاحية التوكن
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (userExists)
                return BadRequest("Email already registered");

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
             //  IsEmailConfirmed = false,
                OTP = ""
            };
          //  newUser.EmailConfirmationToken = Guid.NewGuid().ToString();

            //string confirmationLink = "http://yourapp.com/confirm-email?token=yourToken";
            //_emailService.SendConfirmationEmail(user.Email, confirmationLink);  // ✅ سيتم التعرف عليه الآن
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully. Please check your email.");
           
        }
        //[HttpGet("confirm-email")]
        //public async Task<IActionResult> ConfirmEmail(int userId, string token)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        //    if (user == null)
        //        return BadRequest("Invalid user.");

        //    if (user.EmailConfirmationToken != token)
        //        return BadRequest("Invalid or expired token.");

        //    user.IsEmailConfirmed = true;
        //    user.EmailConfirmationToken = null; // احذف التوكن بعد التحقق
        //    await _context.SaveChangesAsync();

        //    return Ok("Email confirmed successfully. You can now log in.");

        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var userData = await _context.Users
              .Where(u => u.Email == model.Email)
              .Select(u => new
              {
                  UserId = u.UserId,
                  u.Email,
                  u.PasswordHash,
                  u.IsEmailConfirmed
              })
              .FirstOrDefaultAsync();

            if (userData == null || !BCrypt.Net.BCrypt.Verify(model.Password, userData.PasswordHash))
                return Unauthorized("Invalid User Data");

            //if (!userData.IsEmailConfirmed)
            //    return BadRequest("Email not confirmed");

            // ✅ إنشاء كائن `User` قبل تمريره إلى `GenerateJwtToken`
            var user = new User
            {
                UserId = userData.UserId,
                Email = userData.Email,
                PasswordHash = userData.PasswordHash,
                IsEmailConfirmed = userData.IsEmailConfirmed
            };

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

            [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] OTPRequestDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return NotFound("User not found");

            user.OTP = new Random().Next(100000, 999999).ToString();
            await _context.SaveChangesAsync();

            // Send OTP via Email (Implement Email Service)
            return Ok("OTP sent to email");
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerficationDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || user.OTP != model.OTP)
                return BadRequest("Invalid OTP");

           user.IsEmailConfirmed = true;
            user.OTP = null;
            await _context.SaveChangesAsync();

            return Ok("Email verified successfully");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return NotFound("User not found");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _context.SaveChangesAsync();

            return Ok("Password reset successfully");
        }
        [HttpPost("set-gender")]
        public async Task<IActionResult> SetGender([FromBody] GenderDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.UserID);
            if (user == null)
                return NotFound("User not found");

            user.Gender = model.Gender;
            await _context.SaveChangesAsync();

            return Ok("Gender updated successfully");
        }
        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                user = new User { FirstName="",PasswordHash="",Email = model.Email//, IsEmailConfirmed = true 
                };
              //  user.EmailConfirmationToken = user.EmailConfirmationToken ?? Guid.NewGuid().ToString();

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
              //  user.EmailConfirmationToken = user.EmailConfirmationToken ?? Guid.NewGuid().ToString();
                await _context.SaveChangesAsync();
            }
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("User is null or email is missing.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });

        }

       
        
        //[HttpPost("confirm-email")]
        //public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO model)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        //    if (user == null || !user.IsEmailConfirmed)
        //        return BadRequest("Invalid request");

        //    return Ok("Email confirmed successfully");
        //}

    }
}


