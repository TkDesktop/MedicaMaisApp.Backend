using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedicaMaisApp.Backend.Modelos;
using Microsoft.IdentityModel.Tokens;

namespace MedicaMaisApp.Backend.Services
{
    public interface ITokenService
    {
        (string token, DateTime expiraEm) GerarToken(Usuario usuario);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuracao;

        public TokenService(IConfiguration configuracao)
        {
            this.configuracao = configuracao;
        }

        public (string token, DateTime expiraEm) GerarToken(Usuario usuario)
        {
            var chave = configuracao["Jwt:Chave"]
                ?? throw new InvalidOperationException("Jwt:Chave não configurada em appsettings.json");

            var credenciais = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, usuario.Email),
                new("nome", usuario.Nome),
                new(ClaimTypes.Role, usuario.TipoUsuario.ToString())
            };

            var expiraEm = DateTime.UtcNow.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: configuracao["Jwt:Emissor"],
                audience: configuracao["Jwt:Audiencia"],
                claims: claims,
                expires: expiraEm,
                signingCredentials: credenciais);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiraEm);
        }
    }
}
