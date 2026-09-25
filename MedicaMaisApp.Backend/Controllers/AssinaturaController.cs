using MedicaMaisApp.Backend.DTOs;
using MedicaMaisApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    // Equivale a checkout.html (seleção de plano + pagamento por Cartão ou Pix)
    [Route("api/assinaturas")]
    public class AssinaturaController : ControllerBaseAutenticado
    {
        private readonly IAssinaturaService assinaturaService;

        public AssinaturaController(IAssinaturaService assinaturaService)
        {
            this.assinaturaService = assinaturaService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar() =>
            Ok(await assinaturaService.ListarDoUsuarioAsync(UsuarioIdLogado));

        // POST api/assinaturas/checkout -> equivale a clicar em "Pagar" / "Já paguei" em checkout.html
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(AssinaturaCheckoutDto dto)
        {
            var assinatura = await assinaturaService.ContratarAsync(UsuarioIdLogado, dto);
            return CreatedAtAction(nameof(Listar),new { },
            AssinaturaRespostaDto.DeEntidade(assinatura));
        }
    }
}
