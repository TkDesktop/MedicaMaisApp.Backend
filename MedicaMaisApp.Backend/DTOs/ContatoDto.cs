using System.ComponentModel.DataAnnotations;
using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    public class ContatoDto
    {
        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        public TipoContato Tipo { get; set; } = TipoContato.Outro;
    }
}
