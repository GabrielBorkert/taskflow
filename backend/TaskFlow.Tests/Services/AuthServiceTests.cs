using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using TaskFlow.Application.Settings;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Services;

namespace TaskFlow.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly JwtService _jwtService;

        public AuthServiceTests()
        {
            var jwtSettings = Options.Create(new JwtSettings
            {
                SecretKey = "chave-secreta-super-segura-minimo-32-caracteres",
                Issuer = "TaskFlow",
                Audience = "TaskFlowUsers",
                ExpirationMinutes = 60
            });

            _jwtService = new JwtService(jwtSettings);
        }

        private AppDbContext CriarContextoInMemory()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Register_DeveCriarUsuarioComSucesso()
        {
            // Arrange
            var context = CriarContextoInMemory();
            var authService = new AuthService(context, _jwtService);

            // Act
            var resultado = await authService.RegisterAsync("Gabriel", "gabriel@email.com", "123456");

            // Assert
            resultado.Should().Be("Usuário criado com sucesso.");
            context.Users.Should().HaveCount(1);
        }

        [Fact]
        public async Task Register_DeveHashearASenha()
        {
            // Arrange
            var context = CriarContextoInMemory();
            var authService = new AuthService(context, _jwtService);

            // Act
            await authService.RegisterAsync("Gabriel", "gabriel@email.com", "123456");
            var usuario = context.Users.First();

            // Assert
            usuario.PasswordHash.Should().NotBe("123456");
            usuario.PasswordHash.Should().StartWith("$2");
        }

        [Fact]
        public async Task Register_DeveLancarExcecaoComEmailDuplicado()
        {
            // Arrange
            var context = CriarContextoInMemory();
            var authService = new AuthService(context, _jwtService);
            await authService.RegisterAsync("Gabriel", "gabriel@email.com", "123456");

            // Act
            var acao = async () => await authService.RegisterAsync("Outro", "gabriel@email.com", "654321");

            // Assert
            await acao.Should().ThrowAsync<Exception>()
                .WithMessage("Email já cadastrado.");
        }

        [Fact]
        public async Task Login_DeveRetornarTokensComCredenciaisCorretas()
        {
            // Arrange
            var context = CriarContextoInMemory();
            var authService = new AuthService(context, _jwtService);
            await authService.RegisterAsync("Gabriel", "gabriel@email.com", "123456");

            // Act
            var resultado = await authService.LoginAsync("gabriel@email.com", "123456");

            // Assert
            resultado.AccessToken.Should().NotBeNullOrEmpty();
            resultado.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_DeveLancarExcecaoComSenhaErrada()
        {
            // Arrange
            var context = CriarContextoInMemory();
            var authService = new AuthService(context, _jwtService);
            await authService.RegisterAsync("Gabriel", "gabriel@email.com", "123456");

            // Act
            var acao = async () => await authService.LoginAsync("gabriel@email.com", "senhaerrada");

            // Assert
            await acao.Should().ThrowAsync<Exception>()
                .WithMessage("Credenciais inválidas.");
        }

        [Fact]
        public async Task Login_DeveLancarExcecaoComEmailInexistente()
        {
            // Arrange
            var context = CriarContextoInMemory();
            var authService = new AuthService(context, _jwtService);

            // Act
            var acao = async () => await authService.LoginAsync("naoexiste@email.com", "123456");

            // Assert
            await acao.Should().ThrowAsync<Exception>()
                .WithMessage("Credenciais inválidas.");
        }
    }
}