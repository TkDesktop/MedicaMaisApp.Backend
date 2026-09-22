using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaMaisApp.Backend.Controllers
{
    // Todos os controllers que exigem login herdam daqui.
    // O ID do usuário nunca vem da URL/body "solto" - é sempre extraído do token JWT
    // validado pelo middleware de autenticação, evitando que um usuário acesse dados de outro.
    [Authorize]
    [ApiController]
    public abstract class ControllerBaseAutenticado : ControllerBase
    {
        protected int UsuarioIdLogado =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? throw new InvalidOperationException("Token sem identificador de usuário."));
    }
}
