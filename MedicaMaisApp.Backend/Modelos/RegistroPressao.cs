using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicaMaisApp.Backend.Modelos
{
    public class RegistroPressao
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime DataHora { get; set; }

        [Range(40, 300)]
        public int Sistolica { get; set; }

        [Range(20, 200)]
        public int Diastolica { get; set; }

        // Classificação de acordo com a legenda de logado.html:
        // > 140 = Muito alta | 90-120 = Normal | < 60 = Muito baixa
        [NotMapped]
        public string Classificacao =>
            Sistolica > 140 ? "Alta" :
            Sistolica < 60 ? "Baixa" :
            "Normal";
    }
}
