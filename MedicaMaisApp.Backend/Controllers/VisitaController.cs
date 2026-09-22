using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    // Equivale à seção "Suporte prioritário" de logado.html (inclui o botão "Reagendar")
    [Route("api/visitas")]
    public class VisitaController : ControllerBaseAutenticado
    {
        private readonly IVisitaService visitaService;

        public VisitaController(IVisitaService visitaService)
        {
            this.visitaService = visitaService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar() =>
            Ok(await visitaService.ListarDoUsuarioAsync(UsuarioIdLogado));

        // GET api/visitas/proxima  -> equivale ao card "A próxima visita será em:"
        [HttpGet("proxima")]
        public async Task<IActionResult> Proxima()
        {
            var visita = await visitaService.ProximaVisitaAsync(UsuarioIdLogado);
            return visita is null ? NotFound() : Ok(visita);
        }

        [HttpPost]
        public async Task<IActionResult> Agendar(VisitaSuporteCriacaoDto dto)
        {
            var visita = await visitaService.AgendarAsync(UsuarioIdLogado, dto);
            return CreatedAtAction(nameof(Listar), visita);
        }

        // PUT api/visitas/{id}/reagendar  -> equivale ao botão "Reagendar"
        [HttpPut("{id}/reagendar")]
        public async Task<IActionResult> Reagendar(int id, VisitaReagendamentoDto dto)
        {
            var visita = await visitaService.ReagendarAsync(UsuarioIdLogado, id, dto);
            return visita is null ? NotFound() : Ok(visita);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var cancelada = await visitaService.CancelarAsync(UsuarioIdLogado, id);
            return cancelada ? NoContent() : NotFound();
        }
    }
}
