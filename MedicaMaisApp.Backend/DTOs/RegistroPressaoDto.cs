using System.ComponentModel.DataAnnotations;

namespace MedicaMaisApp.Backend.DTOs
{
    public class RegistroPressaoDto
    {
        public DateTime? DataHora { get; set; }

        [Range(40, 300)]
        public int Sistolica { get; set; }

        [Range(20, 200)]
        public int Diastolica { get; set; }
    }
}
