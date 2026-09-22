using System.ComponentModel.DataAnnotations.Schema;

namespace MedicaMaisApp.Backend.Modelos
{
    public class Assinatura
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public Plano Plano { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        public MetodoPagamento MetodoPagamento { get; set; }

        public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;

        public DateTime DataPagamento { get; set; } = DateTime.UtcNow;
    }
}
