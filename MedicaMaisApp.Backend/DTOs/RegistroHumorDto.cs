using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    public class RegistroHumorDto
    {
        public DateTime? DataHora { get; set; }
        public TipoHumor Humor { get; set; }
    }
}
