using System.ComponentModel.DataAnnotations;

namespace MedicaMaisApp.Backend.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(11)]
        public string Cpf { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // Nunca armazenamos a senha em texto puro: guardamos hash + salt (ver Services/SenhaHasher).
        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        [Required]
        public string SenhaSalt { get; set; } = string.Empty;

        public TipoUsuario TipoUsuario { get; set; }

        public Plano Plano { get; set; } = Plano.Essencial;

        public string? FotoUrl { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        // Navegações
        public List<Contato> Contatos { get; set; } = new();
        public List<RegistroPressao> RegistrosPressao { get; set; } = new();
        public List<RegistroHumor> RegistrosHumor { get; set; } = new();
        public List<VisitaSuporte> Visitas { get; set; } = new();
        public List<Assinatura> Assinaturas { get; set; } = new();
    }
}
