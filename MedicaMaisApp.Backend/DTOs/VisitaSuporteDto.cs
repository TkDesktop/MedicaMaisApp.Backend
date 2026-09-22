using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    public class VisitaSuporteCriacaoDto
    {
        public DateTime DataHora { get; set; }
        public string? Observacao { get; set; }
    }

    public class VisitaReagendamentoDto
    {
        public DateTime NovaDataHora { get; set; }
    }
}
