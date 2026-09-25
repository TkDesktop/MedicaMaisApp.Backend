using System.ComponentModel.DataAnnotations;

namespace MedicaMaisApp.Backend.DTOs
{
    public class RegistroPressaoDto : IValidatableObject
    {
        public DateTime? DataHora { get; set; }

        [Range(40, 300)]
        public int Sistolica { get; set; }

        [Range(20, 200)]
        public int Diastolica { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Sistolica <= Diastolica)
            {
                yield return new ValidationResult(
                    "A pressão sistólica deve ser maior que a pressão diastólica.",
                    new[] { nameof(Sistolica), nameof(Diastolica) });
            }
        }
    }
}
