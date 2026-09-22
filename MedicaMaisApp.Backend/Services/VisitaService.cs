using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MedicaMaisApp.Backend.Services
{
    public interface IVisitaService
    {
        Task<List<VisitaSuporte>> ListarDoUsuarioAsync(int usuarioId);
        Task<VisitaSuporte?> ProximaVisitaAsync(int usuarioId);
        Task<VisitaSuporte> AgendarAsync(int usuarioId, VisitaSuporteCriacaoDto dto);
        Task<VisitaSuporte?> ReagendarAsync(int usuarioId, int visitaId, VisitaReagendamentoDto dto);
        Task<bool> CancelarAsync(int usuarioId, int visitaId);
    }

    public class VisitaService : IVisitaService
    {
        private readonly AppDbContext contexto;

        public VisitaService(AppDbContext contexto)
        {
            this.contexto = contexto;
        }

        public Task<List<VisitaSuporte>> ListarDoUsuarioAsync(int usuarioId) =>
            contexto.Visitas.Where(v => v.UsuarioId == usuarioId)
                .OrderBy(v => v.DataHora)
                .ToListAsync();

        public Task<VisitaSuporte?> ProximaVisitaAsync(int usuarioId) =>
            contexto.Visitas
                .Where(v => v.UsuarioId == usuarioId
                    && v.DataHora >= DateTime.UtcNow
                    && v.Status != StatusVisita.Cancelada)
                .OrderBy(v => v.DataHora)
                .FirstOrDefaultAsync();

        public async Task<VisitaSuporte> AgendarAsync(int usuarioId, VisitaSuporteCriacaoDto dto)
        {
            var visita = new VisitaSuporte
            {
                UsuarioId = usuarioId,
                DataHora = dto.DataHora,
                Observacao = dto.Observacao,
                Status = StatusVisita.Agendada
            };

            contexto.Visitas.Add(visita);
            await contexto.SaveChangesAsync();
            return visita;
        }

        public async Task<VisitaSuporte?> ReagendarAsync(int usuarioId, int visitaId, VisitaReagendamentoDto dto)
        {
            var visita = await contexto.Visitas
                .FirstOrDefaultAsync(v => v.UsuarioId == usuarioId && v.Id == visitaId);

            if (visita is null) return null;

            visita.DataHora = dto.NovaDataHora;
            visita.Status = StatusVisita.Reagendada;

            await contexto.SaveChangesAsync();
            return visita;
        }

        public async Task<bool> CancelarAsync(int usuarioId, int visitaId)
        {
            var visita = await contexto.Visitas
                .FirstOrDefaultAsync(v => v.UsuarioId == usuarioId && v.Id == visitaId);

            if (visita is null) return false;

            visita.Status = StatusVisita.Cancelada;
            await contexto.SaveChangesAsync();
            return true;
        }
    }
}
