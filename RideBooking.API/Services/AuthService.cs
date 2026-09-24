using RideBooking.API.Data;
using RideBooking.API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RideBooking.API.Models;

namespace RideBooking.API.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        private readonly PasswordHasher<User> _passwordHasher;
        public AuthService(ApplicationDbContext context,
            ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;

            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> EmailExistsAsync (string email)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            return await _context.Users
                .AnyAsync(u => u.Email == normalizedEmail);
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
        {
            var normalizedEmail = request.Email
                .Trim()
                .ToLowerInvariant();
            if(await EmailExistsAsync(normalizedEmail))
            {
                _logger.LogWarning(
                    "Registration attempt with existing email: {Email}",
                    normalizedEmail);

                return new RegisterResult
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = normalizedEmail,
                PasswordHash = _passwordHasher.HashPassword(
                    null!,
                    request.Password),
                Role = "Customer",
                CreateAt = DateTime.UtcNow

            };

            try
            {
              _context.Users.Add(user);
              await _context.SaveChangesAsync();

                _logger.LogInformation(
                        "User registered successfully. UserId: {UserId}, Email: {Email}",
                        user.Id,
                        user.Email);

                return new RegisterResult
                {
                    Success = true,
                    Message = "Registration successful."
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while registering user with email: {Email}",
                    normalizedEmail);

                return new RegisterResult
                {
                    Success = false,
                    Message = "Unable to save user information."
                };
            }
        }

    }
}
