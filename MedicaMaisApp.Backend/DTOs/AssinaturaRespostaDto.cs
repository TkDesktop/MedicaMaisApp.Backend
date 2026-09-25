using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    public class AssinaturaRespostaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Plano Plano { get; set; }
        public decimal Valor { get; set; }
        public MetodoPagamento MetodoPagamento { get; set; }
        public StatusPagamento Status { get; set; }
        public DateTime DataPagamento { get; set; }

        public static AssinaturaRespostaDto DeEntidade(Assinatura assinatura) => new()
        {
            Id = assinatura.Id,
            UsuarioId = assinatura.UsuarioId,
            Plano = assinatura.Plano,
            Valor = assinatura.Valor,
            MetodoPagamento = assinatura.MetodoPagamento,
            Status = assinatura.Status,
            DataPagamento = assinatura.DataPagamento
        };
    }
}
