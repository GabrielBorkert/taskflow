using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Templates;

namespace TaskFlow.Infrastructure.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, JwtService jwtService, IEmailService emailService)
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<string> RegisterAsync(string name, string email, string password)
        {
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            if (emailExists)
                throw new Exception("Email já cadastrado.");

            var user = new UserEntity
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Envia email de boas vindas
            var htmlBody = WelcomeEmailTemplate.Generate(name);
            await _emailService.SendEmailAsync(email, name, "Bem-vindo ao TaskFlow!", htmlBody);

            return "Usuário criado com sucesso.";
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Credenciais inválidas.");

            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResponse(accessToken, refreshToken);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Refresh token inválido ou expirado.");

            tokenEntity.IsRevoked = true;

            var newAccessToken = _jwtService.GenerateToken(tokenEntity.User);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshTokenEntity
            {
                Token = newRefreshToken,
                UserId = tokenEntity.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResponse(newAccessToken, newRefreshToken);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (tokenEntity == null)
                throw new Exception("Token não encontrado.");

            tokenEntity.IsRevoked = true;
            await _context.SaveChangesAsync();
        }

        public async Task<string> RegisterAdminAsync(string name, string email, string password)
        {
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            if (emailExists)
                throw new Exception("Email já cadastrado.");

            var user = new UserEntity
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Administrador criado com sucesso.";
        }
    }
}