using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    // Equivale à seção "Registros de humor" de logado.html
    [Route("api/humor")]
    public class HumorController : ControllerBaseAutenticado
    {
        private readonly IHumorService humorService;

        public HumorController(IHumorService humorService)
        {
            this.humorService = humorService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] DateTime? inicio, [FromQuery] DateTime? fim) =>
            Ok(await humorService.ListarPorPeriodoAsync(UsuarioIdLogado, inicio, fim));

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroHumorDto dto)
        {
            var registro = await humorService.RegistrarAsync(UsuarioIdLogado, dto);
            return CreatedAtAction(nameof(Listar), registro);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await humorService.ExcluirAsync(UsuarioIdLogado, id);
            return excluido ? NoContent() : NotFound();
        }
    }
}
