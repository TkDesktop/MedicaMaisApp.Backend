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

    public class VisitaReagendamentoDto : IValidatableObject
    {
        public DateTime NovaDataHora { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NovaDataHora <= DateTime.UtcNow)
            {
                yield return new ValidationResult(
                    "A nova data e hora da visita devem estar no futuro.",
                    new[] { nameof(NovaDataHora) });
            }
        }
    }
}
