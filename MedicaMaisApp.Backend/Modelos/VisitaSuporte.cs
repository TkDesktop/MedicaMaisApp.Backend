namespace MedicaMaisApp.Backend.Modelos
{
    public class VisitaSuporte
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime DataHora { get; set; }

        public StatusVisita Status { get; set; } = StatusVisita.Agendada;

        public string? Observacao { get; set; }
    }
}
