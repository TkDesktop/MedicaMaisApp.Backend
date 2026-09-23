using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    // O que a API devolve sobre um usuário — nunca inclui senha/hash/salt.
    public class UsuarioRespostaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string Email { get; set; } = string.Empty;
        public TipoUsuario TipoUsuario { get; set; }
        public Plano? Plano { get; set; }
        public string? FotoUrl { get; set; }
        public DateTime DataCadastro { get; set; }

        public static UsuarioRespostaDto DeEntidade(Usuario u) => new()
        {
            Id = u.Id,
            Nome = u.Nome,
            Cpf = u.Cpf,
            Telefone = u.Telefone,
            Email = u.Email,
            TipoUsuario = u.TipoUsuario,
            Plano = u.Plano,
            FotoUrl = u.FotoUrl
        };
    }
}
