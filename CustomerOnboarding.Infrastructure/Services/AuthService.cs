using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerOnboarding.Infrastructure.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly JwtTokenService _jwt;

        public AuthService(AppDbContext db, JwtTokenService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        public async Task<AppUser?> RegisterAsync(string username, string password, string role)
        {
            if (await _db.Users.AnyAsync(u => u.Username == username))
                return null;

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<AuthResponseDto?> LoginAsync(string username, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return _jwt.GenerateToken(user);
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }
    }
}
