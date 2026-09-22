using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MedicaMaisApp.Backend.Services
{
    public interface IHumorService
    {
        Task<List<RegistroHumor>> ListarPorPeriodoAsync(int usuarioId, DateTime? inicio, DateTime? fim);
        Task<RegistroHumor> RegistrarAsync(int usuarioId, RegistroHumorDto dto);
        Task<bool> ExcluirAsync(int usuarioId, int registroId);
    }

    public class HumorService : IHumorService
    {
        private readonly AppDbContext contexto;

        public HumorService(AppDbContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<List<RegistroHumor>> ListarPorPeriodoAsync(int usuarioId, DateTime? inicio, DateTime? fim)
        {
            var consulta = contexto.RegistrosHumor.Where(h => h.UsuarioId == usuarioId);

            if (inicio.HasValue)
                consulta = consulta.Where(h => h.DataHora >= inicio.Value);

            if (fim.HasValue)
                consulta = consulta.Where(h => h.DataHora <= fim.Value);

            return await consulta.OrderBy(h => h.DataHora).ToListAsync();
        }

        public async Task<RegistroHumor> RegistrarAsync(int usuarioId, RegistroHumorDto dto)
        {
            var registro = new RegistroHumor
            {
                UsuarioId = usuarioId,
                DataHora = dto.DataHora ?? DateTime.UtcNow,
                Humor = dto.Humor
            };

            contexto.RegistrosHumor.Add(registro);
            await contexto.SaveChangesAsync();
            return registro;
        }

        public async Task<bool> ExcluirAsync(int usuarioId, int registroId)
        {
            var registro = await contexto.RegistrosHumor
                .FirstOrDefaultAsync(h => h.UsuarioId == usuarioId && h.Id == registroId);

            if (registro is null) return false;

            contexto.RegistrosHumor.Remove(registro);
            await contexto.SaveChangesAsync();
            return true;
        }
    }
}
