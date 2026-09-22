using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    // Equivale à seção "Contatos" de logado.html
    [Route("api/contatos")]
    public class ContatoController : ControllerBaseAutenticado
    {
        private readonly IContatoService contatoService;

        public ContatoController(IContatoService contatoService)
        {
            this.contatoService = contatoService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar() =>
            Ok(await contatoService.ListarDoUsuarioAsync(UsuarioIdLogado));

        [HttpGet("{id}")]
        public async Task<IActionResult> Buscar(int id)
        {
            var contato = await contatoService.BuscarAsync(UsuarioIdLogado, id);
            return contato is null ? NotFound() : Ok(contato);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(ContatoDto dto)
        {
            var contato = await contatoService.CriarAsync(UsuarioIdLogado, dto);
            return CreatedAtAction(nameof(Buscar), new { id = contato.Id }, contato);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, ContatoDto dto)
        {
            var contato = await contatoService.AtualizarAsync(UsuarioIdLogado, id, dto);
            return contato is null ? NotFound() : Ok(contato);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await contatoService.ExcluirAsync(UsuarioIdLogado, id);
            return excluido ? NoContent() : NotFound();
        }
    }
}
