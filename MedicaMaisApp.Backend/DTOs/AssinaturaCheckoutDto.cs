using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    // Usado em POST /api/assinaturas/checkout (equivale a checkout.html)
    public class AssinaturaCheckoutDto
    {
        public Plano Plano { get; set; }
        public MetodoPagamento MetodoPagamento { get; set; }
    }
}
