namespace MedicaMaisApp.Backend.DTOs
{
    public class TokenRespostaDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
        public UsuarioRespostaDto Usuario { get; set; } = null!;
    }
}
