using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Infrastructure.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
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

            return "Usuário criado com sucesso.";
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Credenciais inválidas.");

            return _jwtService.GenerateToken(user);
        }
    }
}