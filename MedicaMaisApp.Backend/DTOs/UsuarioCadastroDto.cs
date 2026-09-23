using System.ComponentModel.DataAnnotations;
using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    // Usado em POST /api/auth/cadastro (equivale ao form-cadastro de login.html)
    public class UsuarioCadastroDto
    {
        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, MinLength(11), MaxLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Senha { get; set; } = string.Empty;

        public TipoUsuario TipoUsuario { get; set; }
    }
}
