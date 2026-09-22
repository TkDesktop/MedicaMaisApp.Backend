using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MedicaMaisApp.Backend.Services
{
    public interface IContatoService
    {
        Task<List<Contato>> ListarDoUsuarioAsync(int usuarioId);
        Task<Contato?> BuscarAsync(int usuarioId, int contatoId);
        Task<Contato> CriarAsync(int usuarioId, ContatoDto dto);
        Task<Contato?> AtualizarAsync(int usuarioId, int contatoId, ContatoDto dto);
        Task<bool> ExcluirAsync(int usuarioId, int contatoId);
    }

    public class ContatoService : IContatoService
    {
        private readonly AppDbContext contexto;

        public ContatoService(AppDbContext contexto)
        {
            this.contexto = contexto;
        }

        public Task<List<Contato>> ListarDoUsuarioAsync(int usuarioId) =>
            contexto.Contatos.Where(c => c.UsuarioId == usuarioId).ToListAsync();

        public Task<Contato?> BuscarAsync(int usuarioId, int contatoId) =>
            contexto.Contatos.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.Id == contatoId);

        public async Task<Contato> CriarAsync(int usuarioId, ContatoDto dto)
        {
            var contato = new Contato
            {
                UsuarioId = usuarioId,
                Nome = dto.Nome.Trim(),
                Telefone = dto.Telefone,
                Tipo = dto.Tipo
            };

            contexto.Contatos.Add(contato);
            await contexto.SaveChangesAsync();
            return contato;
        }

        public async Task<Contato?> AtualizarAsync(int usuarioId, int contatoId, ContatoDto dto)
        {
            var contato = await BuscarAsync(usuarioId, contatoId);
            if (contato is null) return null;

            contato.Nome = dto.Nome.Trim();
            contato.Telefone = dto.Telefone;
            contato.Tipo = dto.Tipo;

            await contexto.SaveChangesAsync();
            return contato;
        }

        public async Task<bool> ExcluirAsync(int usuarioId, int contatoId)
        {
            var contato = await BuscarAsync(usuarioId, contatoId);
            if (contato is null) return false;

            contexto.Contatos.Remove(contato);
            await contexto.SaveChangesAsync();
            return true;
        }
    }
}
