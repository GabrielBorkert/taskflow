using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs
{
    public record RegisterRequest(
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        string Name,

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        string Email,

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        string Password
    );

    public record LoginRequest(
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        string Email,

        [Required(ErrorMessage = "Senha é obrigatória")]
        string Password
    );

    public record RefreshRequest(
        [Required(ErrorMessage = "Refresh token é obrigatório")]
        string RefreshToken
    );

}