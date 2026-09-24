namespace MedicaMaisApp.Backend.Services
{
    public class EmailService : IEmailService
    {
        public Task EnviarCodigoConfirmacaoAsync(string email, string codigo)
        {
            Console.WriteLine($"Código de confirmação para {email}: {codigo}");

            return Task.CompletedTask;
        }
    }
}