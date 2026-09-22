using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    // Equivale ao "formPerfil" / overlay de perfil em logado.html
    [Route("api/usuarios/me")]
    public class UsuarioController : ControllerBaseAutenticado
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        // GET api/usuarios/me
        [HttpGet]
        public async Task<ActionResult<UsuarioRespostaDto>> ObterPerfil()
        {
            var usuario = await usuarioService.BuscarPorIdAsync(UsuarioIdLogado);

            if (usuario is null) return NotFound();

            return UsuarioRespostaDto.DeEntidade(usuario);
        }

        // PUT api/usuarios/me
        [HttpPut]
        public async Task<ActionResult<UsuarioRespostaDto>> AtualizarPerfil(UsuarioAtualizacaoDto dto)
        {
            var (sucesso, erro, usuario) = await usuarioService.AtualizarAsync(UsuarioIdLogado, dto);

            if (!sucesso || usuario is null)
                return BadRequest(new { mensagem = erro });

            return UsuarioRespostaDto.DeEntidade(usuario);
        }

        // DELETE api/usuarios/me  -> exclui a própria conta
        [HttpDelete]
        public async Task<IActionResult> ExcluirConta()
        {
            var excluido = await usuarioService.ExcluirAsync(UsuarioIdLogado);
            return excluido ? NoContent() : NotFound();
        }
    }
}
