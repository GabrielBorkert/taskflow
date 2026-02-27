using FluentAssertions;
using Microsoft.Extensions.Options;
using TaskFlow.Application.Settings;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Services;

namespace TaskFlow.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;

        public JwtServiceTests()
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

        [Fact]
        public void GenerateToken_DeveRetornarTokenNaoVazio()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = 1,
                Name = "Gabriel",
                Email = "gabriel@email.com",
                Role = "User"
            };

            // Act
            var token = _jwtService.GenerateToken(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateToken_DeveTerTresPartesSeparadasPorPonto()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = 1,
                Name = "Gabriel",
                Email = "gabriel@email.com",
                Role = "User"
            };

            // Act
            var token = _jwtService.GenerateToken(user);
            var partes = token.Split('.');

            // Assert
            partes.Should().HaveCount(3);
        }

        [Fact]
        public void GenerateRefreshToken_DeveRetornarTokenNaoVazio()
        {
            // Act
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Assert
            refreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateRefreshToken_DoisTokensNaoDevemSerIguais()
        {
            // Act
            var token1 = _jwtService.GenerateRefreshToken();
            var token2 = _jwtService.GenerateRefreshToken();

            // Assert
            token1.Should().NotBe(token2);
        }
    }
}