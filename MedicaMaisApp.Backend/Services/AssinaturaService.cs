using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MedicaMaisApp.Backend.Services
{
    public interface IAssinaturaService
    {
        Task<Assinatura> ContratarAsync(int usuarioId, AssinaturaCheckoutDto dto);
        Task<List<Assinatura>> ListarDoUsuarioAsync(int usuarioId);
    }

    public class AssinaturaService : IAssinaturaService
    {
        private readonly AppDbContext contexto;

        // Preços fixos, iguais aos exibidos em index.html.
        // Em um cenário real isso viria de uma tabela "Planos" no banco.
        private static readonly Dictionary<Plano, decimal> Precos = new()
        {
            [Plano.Essencial] = 0.00m,
            [Plano.Cuidado] = 59.90m,
            [Plano.CuidadoTotal] = 699.99m
        };

        public AssinaturaService(AppDbContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<Assinatura> ContratarAsync(int usuarioId, AssinaturaCheckoutDto dto)
        {
            var usuario = await contexto.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId)
                ?? throw new InvalidOperationException("Usuário não encontrado.");

            var valor = Precos[dto.Plano];

            // Simulação de gateway de pagamento (equivalente ao checkout.html, que sempre
            // "aprova" o pagamento). Plano Essencial não passa por cobrança.
            var assinatura = new Assinatura
            {
                UsuarioId = usuarioId,
                Plano = dto.Plano,
                Valor = valor,
                MetodoPagamento = dto.MetodoPagamento,
                Status = StatusPagamento.Aprovado,
                DataPagamento = DateTime.UtcNow
            };

            contexto.Assinaturas.Add(assinatura);

            // Ativa o plano no usuário assim que o pagamento é aprovado.
            usuario.Plano = dto.Plano;

            await contexto.SaveChangesAsync();
            return assinatura;
        }

        public Task<List<Assinatura>> ListarDoUsuarioAsync(int usuarioId) =>
            contexto.Assinaturas
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.DataPagamento)
                .ToListAsync();
    }
}
