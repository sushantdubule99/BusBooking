using Azure.Core;
using busBooking.Models;
using BusBookingApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace busBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BusBookingDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(
            BusBookingDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


       

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer(CustomerRequest request)
        {
            var existingUser = await _context.UserTbl
       .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (existingUser != null)
                return BadRequest("Email already registered");

            // Create new customer
            var user = new UserTbl
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,

                // Hash the password before storing it
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),

                Role = "Customer",
                ApprovalStatus = "Approved",
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _context.UserTbl.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Customer registered successfully"
            });
        }

        [HttpPost("register-partner")]
        public async Task<IActionResult> RegisterPartner(CustomerRequest user)
        {
            var existingUser = await _context.UserTbl
                .FirstOrDefaultAsync(x => x.Email == user.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            var newUser = new UserTbl
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(user.Password),

                Role = "TravelPartner",

                ApprovalStatus = "Pending",

                IsActive = true,

                CreatedDate = DateTime.Now
            };

            _context.UserTbl.Add(newUser);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Travel Partner registration submitted for approval",
                userId = newUser.UserId,
                role = newUser.Role,
                approvalStatus = newUser.ApprovalStatus
            });
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            // Find user by email
            var user = await _context.UserTbl
                .FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            Console.WriteLine("Email: " + user.Email);
            Console.WriteLine("Hash: " + user.PasswordHash);


            // Verify password
            bool passwordValid = BCrypt.Net.BCrypt.Verify(
                loginRequest.Password,
                user.PasswordHash
            );
            Console.WriteLine("Password valid: " + passwordValid);

            var testHash = BCrypt.Net.BCrypt.HashPassword("12345");

            bool testResult = BCrypt.Net.BCrypt.Verify(
                "12345",
                testHash
            );

            Console.WriteLine("BCrypt test result: " + testResult);


            if (!passwordValid)
            {
                return Unauthorized("Invalid email or password");
            }


            // Check active account
            if (!user.IsActive)
            {
                return Unauthorized("User account is inactive");
            }


            // Check Travel Partner approval
            if (user.Role == "TravelPartner" &&
                user.ApprovalStatus != "Approved")
            {
                return Unauthorized(
                    "Travel Partner account is not approved yet"
                );
            }


            

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Name
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role
                )
            };


         

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );


         

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


           

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],

                audience: _configuration["Jwt:Audience"],

                claims: claims,

                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        _configuration["Jwt:ExpiryMinutes"]
                    )
                ),

                signingCredentials: credentials
            );


            
            var tokenString =
                new JwtSecurityTokenHandler()
                .WriteToken(token);


            

            return Ok(new
            {
                message = "Login successful",

                token = tokenString,

                userId = user.UserId,

                name = user.Name,

                email = user.Email,

                role = user.Role
            });
        }
    }
}