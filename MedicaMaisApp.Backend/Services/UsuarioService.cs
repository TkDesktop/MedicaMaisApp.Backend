using MedicaMaisApp.Backend.Data;
using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.EntityFrameworkCore;
using Resend;

namespace MedicaMaisApp.Backend.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> BuscarPorIdAsync(int id);
        Task<Usuario?> BuscarPorEmailAsync(string email);
        Task<(bool sucesso, string mensagem)> ConfirmarEmailAsync(ConfirmarEmailDto dto);
        Task<(bool sucesso, string? erro, Usuario? usuario)> CadastrarAsync(UsuarioCadastroDto dto);
        Task<(bool sucesso, Usuario? usuario)> ValidarLoginAsync(UsuarioLoginDto dto);
        Task<(bool sucesso, string? erro, Usuario? usuario)> AtualizarAsync(int id, UsuarioAtualizacaoDto dto);
        Task<bool> ExcluirAsync(int id);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext contexto;
        private readonly IEmailService emailService;

        public UsuarioService(AppDbContext contexto, IEmailService emailService)
        {
            this.contexto = contexto;
            this.emailService = emailService;
        }

        public Task<Usuario?> BuscarPorIdAsync(int id) => contexto.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public Task<Usuario?> BuscarPorEmailAsync(string email) => contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email.ToLower());


        public async Task<(bool sucesso, string? erro, Usuario? usuario)> CadastrarAsync(UsuarioCadastroDto dto)
        {

            if (!CpfValidator.EhValido(dto.Cpf))
            {
                return (false, "CPF inválido.", null);
            }

            var cpfNormalizado = CpfValidator.Normalizar(dto.Cpf);

            var emailNormalizado = dto.Email.Trim().ToLower();

            bool emailEmUso = await contexto.Usuarios.AnyAsync(u => u.Email == emailNormalizado);
            if (emailEmUso)
                return (false, "Este email já está cadastrado.", null);

            bool cpfEmUso = await contexto.Usuarios.AnyAsync(u => u.Cpf == cpfNormalizado);
            if (cpfEmUso)
                return (false, "Este CPF já está cadastrado.", null);

            var codigoConfirmacao = Random.Shared.Next(100000, 1000000).ToString();

            var (hash, salt) = SenhaHasher.Gerar(dto.Senha);

            var usuario = new Usuario
            {
                Nome = dto.Nome.Trim(),
                Cpf = cpfNormalizado,
                Telefone = dto.Telefone,
                Email = emailNormalizado,
                SenhaHash = hash,
                SenhaSalt = salt,
                TipoUsuario = dto.TipoUsuario,

                EmailConfirmado = false,
                CodigoConfirmacaoEmail = codigoConfirmacao,
                ExpiracaoCodigoConfirmacao = DateTime.UtcNow.AddMinutes(15)
            };

            contexto.Usuarios.Add(usuario);
            await contexto.SaveChangesAsync();
            await emailService.EnviarCodigoConfirmacaoAsync(usuario.Email,codigoConfirmacao);

            return (true, null, usuario);
        }
        
        public async Task<(bool sucesso, string mensagem)> ConfirmarEmailAsync(ConfirmarEmailDto dto)
        {
            var emailNormalizado = dto.Email.Trim().ToLower();

            var usuario = await contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Email == emailNormalizado);

            if (usuario is null)
                return (false, "Usuário não encontrado.");

            if (usuario.EmailConfirmado)
                return (false, "Este email já foi confirmado.");

            if (usuario.CodigoConfirmacaoEmail != dto.Codigo)
                return (false, "Código de confirmação inválido.");

            if (usuario.ExpiracaoCodigoConfirmacao is null ||
                usuario.ExpiracaoCodigoConfirmacao < DateTime.UtcNow)
                return (false, "O código de confirmação expirou.");

            usuario.EmailConfirmado = true;
            usuario.CodigoConfirmacaoEmail = null;
            usuario.ExpiracaoCodigoConfirmacao = null;

            await contexto.SaveChangesAsync();

            return (true, "Email confirmado com sucesso.");
        }

        public async Task<(bool sucesso, Usuario? usuario)> ValidarLoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await BuscarPorEmailAsync(dto.Email);

            if (usuario is null)
                return (false, null);

            if (!usuario.EmailConfirmado)
                return (false, null);

            bool senhaValida = SenhaHasher.Validar(dto.Senha, usuario.SenhaHash, usuario.SenhaSalt);

            return senhaValida ? (true, usuario) : (false, null);
        }

        public async Task<(bool sucesso, string? erro, Usuario? usuario)> AtualizarAsync(int id,UsuarioAtualizacaoDto dto)
        {
            var usuario = await contexto.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
                return (false, "Usuário não encontrado.", null);

            var emailNormalizado = dto.Email.Trim().ToLower();

            bool emailEmUso = await contexto.Usuarios.AnyAsync(u => u.Email == emailNormalizado && u.Id != id);

            if (emailEmUso)
                return (false, "Este email já está em uso por outra conta.", null);

            bool emailMudou = usuario.Email != emailNormalizado;

            if (emailMudou)
            {
                var codigoConfirmacao = Random.Shared.Next(100000, 1000000).ToString();

                var expiracaoCodigo = DateTime.UtcNow.AddMinutes(15);

                try
                {
                    await emailService.EnviarCodigoConfirmacaoAsync(emailNormalizado,codigoConfirmacao);
                }
                catch (ResendException)
                {
                    return (false,"Não foi possível enviar o código de confirmação para o novo email.",null);
                }

                usuario.Email = emailNormalizado;
                usuario.EmailConfirmado = false;
                usuario.CodigoConfirmacaoEmail = codigoConfirmacao;
                usuario.ExpiracaoCodigoConfirmacao = expiracaoCodigo;
            }

            usuario.Telefone = dto.Telefone.Trim();
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
