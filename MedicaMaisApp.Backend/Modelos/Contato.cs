using System.ComponentModel.DataAnnotations;

namespace MedicaMaisApp.Backend.Modelos
{
    public class Contato
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        public TipoContato Tipo { get; set; } = TipoContato.Outro;
    }
}
