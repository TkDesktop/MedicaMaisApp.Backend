using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MedicaMaisApp.Backend.Services
{
    public interface IPressaoService
    {
        Task<List<RegistroPressao>> ListarPorPeriodoAsync(int usuarioId, DateTime? inicio, DateTime? fim);
        Task<RegistroPressao> RegistrarAsync(int usuarioId, RegistroPressaoDto dto);
        Task<bool> ExcluirAsync(int usuarioId, int registroId);
    }

    public class PressaoService : IPressaoService
    {
        private readonly AppDbContext contexto;

        public PressaoService(AppDbContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<List<RegistroPressao>> ListarPorPeriodoAsync(int usuarioId, DateTime? inicio, DateTime? fim)
        {
            var consulta = contexto.RegistrosPressao.Where(p => p.UsuarioId == usuarioId);

            if (inicio.HasValue)
                consulta = consulta.Where(p => p.DataHora >= inicio.Value);

            if (fim.HasValue)
                consulta = consulta.Where(p => p.DataHora <= fim.Value);

            return await consulta.OrderBy(p => p.DataHora).ToListAsync();
        }

        public async Task<RegistroPressao> RegistrarAsync(int usuarioId, RegistroPressaoDto dto)
        {
            var registro = new RegistroPressao
            {
                UsuarioId = usuarioId,
                DataHora = dto.DataHora ?? DateTime.UtcNow,
                Sistolica = dto.Sistolica,
                Diastolica = dto.Diastolica
            };

            contexto.RegistrosPressao.Add(registro);
            await contexto.SaveChangesAsync();
            return registro;
        }

        public async Task<bool> ExcluirAsync(int usuarioId, int registroId)
        {
            var registro = await contexto.RegistrosPressao
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.Id == registroId);

            if (registro is null) return false;

            contexto.RegistrosPressao.Remove(registro);
            await contexto.SaveChangesAsync();
            return true;
        }
    }
}
