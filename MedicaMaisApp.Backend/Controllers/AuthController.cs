using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;
        private readonly ITokenService tokenService;

        public AuthController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            this.usuarioService = usuarioService;
            this.tokenService = tokenService;
        }

        // POST api/auth/cadastro  -> equivale ao form-cadastro em login.html
        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastrar(UsuarioCadastroDto dto)
        {
            var (sucesso, erro, usuario) = await usuarioService.CadastrarAsync(dto);

            if (!sucesso || usuario is null)
                return BadRequest(new { mensagem = erro });

            return Created("", new
            {
                mensagem = "Cadastro realizado. Confirme seu email para realizar o login.",
                codigoConfirmacao = usuario.CodigoConfirmacaoEmail,
                usuario = UsuarioRespostaDto.DeEntidade(usuario)
            });
        }

        // POST api/auth/login  -> equivale ao login-form em login.html
        [HttpPost("login")]
        public async Task<ActionResult<TokenRespostaDto>> Login(UsuarioLoginDto dto)
        {
            var (sucesso, usuario) = await usuarioService.ValidarLoginAsync(dto);

            if (!sucesso || usuario is null)
                return Unauthorized(new { mensagem = "Email ou senha inválidos." });

            var (token, expiraEm) = tokenService.GerarToken(usuario);

            return Ok(new TokenRespostaDto
            {
                Token = token,
                ExpiraEm = expiraEm,
                Usuario = UsuarioRespostaDto.DeEntidade(usuario)
            });
        }

        [HttpPost("confirmar-email")]
        public async Task<IActionResult> ConfirmarEmail(ConfirmarEmailDto dto)
        {
            var (sucesso, mensagem) = await usuarioService.ConfirmarEmailAsync(dto);

            if (!sucesso)
                return BadRequest(new { mensagem });

            return Ok(new { mensagem });
        }
    }
}
