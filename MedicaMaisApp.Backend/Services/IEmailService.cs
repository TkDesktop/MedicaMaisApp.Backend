namespace MedicaMaisApp.Backend.Services
{
    public interface IEmailService
    {
        Task EnviarCodigoConfirmacaoAsync(string email, string codigo);
    }
}