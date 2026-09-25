using System.ComponentModel.DataAnnotations;
using MedicaMaisApp.Backend.Modelos;

namespace MedicaMaisApp.Backend.DTOs
{
    public class RegistroHumorDto : IValidatableObject
    {
        public DateTime? DataHora { get; set; }
        public TipoHumor Humor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Enum.IsDefined(typeof(TipoHumor), Humor))
            {
                yield return new ValidationResult(
                    "O tipo de humor informado é inválido.",
                    new[] { nameof(Humor) });
            }
        }
    }
}
