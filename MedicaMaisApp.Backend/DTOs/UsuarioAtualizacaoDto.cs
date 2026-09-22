using System.ComponentModel.DataAnnotations;
using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    // Usado em PUT /api/usuarios/{id} (equivale ao formPerfil de logado.html)
    public class UsuarioAtualizacaoDto
    {
        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, MinLength(11), MaxLength(11)]
        public string Cpf { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        public TipoUsuario TipoUsuario { get; set; }

        public string? FotoUrl { get; set; }
    }
}
