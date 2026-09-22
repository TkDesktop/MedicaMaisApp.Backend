using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    // Equivale à seção "MONITORAMENTO / Picos de Pressão" de logado.html
    [Route("api/pressao")]
    public class PressaoController : ControllerBaseAutenticado
    {
        private readonly IPressaoService pressaoService;

        public PressaoController(IPressaoService pressaoService)
        {
            this.pressaoService = pressaoService;
        }

        // GET api/pressao?inicio=2026-05-10&fim=2026-05-16  -> equivale ao filtro de período do gráfico
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] DateTime? inicio, [FromQuery] DateTime? fim) =>
            Ok(await pressaoService.ListarPorPeriodoAsync(UsuarioIdLogado, inicio, fim));

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroPressaoDto dto)
        {
            var registro = await pressaoService.RegistrarAsync(UsuarioIdLogado, dto);
            return CreatedAtAction(nameof(Listar), registro);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await pressaoService.ExcluirAsync(UsuarioIdLogado, id);
            return excluido ? NoContent() : NotFound();
        }
    }
}
