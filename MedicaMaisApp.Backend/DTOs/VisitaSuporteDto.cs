using MedicaMaisApp.Backend.Modelos;
using System.ComponentModel.DataAnnotations;

namespace MedicaMaisApp.Backend.DTOs
{
    public class VisitaSuporteCriacaoDto : IValidatableObject
    {
        public DateTime DataHora { get; set; }
        public string? Observacao { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataHora <= DateTime.UtcNow)
            {
                yield return new ValidationResult("A data e hora da visita devem estar no futuro.",new[] { nameof(DataHora) });
            }
        }
    }

    public class VisitaReagendamentoDto
    {
        public DateTime NovaDataHora { get; set; }
    }
}
