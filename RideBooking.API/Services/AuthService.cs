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
        private readonly PasswordHasher<User> _passwordHasher;
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> EmailExistsAsync (string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if(await EmailExistsAsync(request.Email))
            {
                return false;
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(
                    null!,
                    request.Password),
                Role = "Customer",
                CreateAt = DateTime.UtcNow

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
