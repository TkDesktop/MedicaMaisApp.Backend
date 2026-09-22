using System.ComponentModel.DataAnnotations;

namespace MedicaMaisApp.Backend.DTOs
{
    // Usado em POST /api/auth/login
    public class UsuarioLoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}
