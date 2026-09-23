using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MedicaMaisApp.Backend.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> BuscarPorIdAsync(int id);
        Task<Usuario?> BuscarPorEmailAsync(string email);
        Task<(bool sucesso, string? erro, Usuario? usuario)> CadastrarAsync(UsuarioCadastroDto dto);
        Task<(bool sucesso, Usuario? usuario)> ValidarLoginAsync(UsuarioLoginDto dto);
        Task<(bool sucesso, string? erro, Usuario? usuario)> AtualizarAsync(int id, UsuarioAtualizacaoDto dto);
        Task<bool> ExcluirAsync(int id);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext contexto;

        public UsuarioService(AppDbContext contexto)
        {
            this.contexto = contexto;
        }

        public Task<Usuario?> BuscarPorIdAsync(int id) =>
            contexto.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public Task<Usuario?> BuscarPorEmailAsync(string email) =>
            contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email.ToLower());

        public async Task<(bool sucesso, string? erro, Usuario? usuario)> CadastrarAsync(UsuarioCadastroDto dto)
        {

            if (!CpfValidator.EhValido(dto.Cpf))
            {
                return (false, "CPF inválido.", null);
            }
                

            var emailNormalizado = dto.Email.Trim().ToLower();

            bool emailEmUso = await contexto.Usuarios.AnyAsync(u => u.Email == emailNormalizado);
            if (emailEmUso)
                return (false, "Este email já está cadastrado.", null);

            bool cpfEmUso = await contexto.Usuarios.AnyAsync(u => u.Cpf == dto.Cpf);
            if (cpfEmUso)
                return (false, "Este CPF já está cadastrado.", null);

            var (hash, salt) = SenhaHasher.Gerar(dto.Senha);

            var usuario = new Usuario
            {
                Nome = dto.Nome.Trim(),
                Cpf = dto.Cpf,
                Telefone = dto.Telefone,
                Email = emailNormalizado,
                SenhaHash = hash,
                SenhaSalt = salt,
                TipoUsuario = dto.TipoUsuario
            };

            contexto.Usuarios.Add(usuario);
            await contexto.SaveChangesAsync();

            return (true, null, usuario);
        }

        public async Task<(bool sucesso, Usuario? usuario)> ValidarLoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await BuscarPorEmailAsync(dto.Email);

            if (usuario is null)
                return (false, null);

            bool senhaValida = SenhaHasher.Validar(dto.Senha, usuario.SenhaHash, usuario.SenhaSalt);

            return senhaValida ? (true, usuario) : (false, null);
        }

        public async Task<(bool sucesso, string? erro, Usuario? usuario)> AtualizarAsync(int id, UsuarioAtualizacaoDto dto)
        {
            var usuario = await contexto.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario is null)
                return (false, "Usuário não encontrado.", null);

            var emailNormalizado = dto.Email.Trim().ToLower();

            bool emailEmUso = await contexto.Usuarios
                .AnyAsync(u => u.Email == emailNormalizado && u.Id != id);
            if (emailEmUso)
                return (false, "Este email já está em uso por outra conta.", null);

            usuario.Nome = dto.Nome.Trim();
            usuario.Cpf = dto.Cpf;
            usuario.Telefone = dto.Telefone;
            usuario.Email = emailNormalizado;
            usuario.TipoUsuario = dto.TipoUsuario;
            usuario.FotoUrl = dto.FotoUrl;

            await contexto.SaveChangesAsync();

            return (true, null, usuario);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var usuario = await contexto.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario is null)
                return false;

            contexto.Usuarios.Remove(usuario);
            await contexto.SaveChangesAsync();
            return true;
        }
    }
}
