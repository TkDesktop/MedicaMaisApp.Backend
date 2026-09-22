namespace MedicaMaisApp.Backend.Modelos
{
    public class RegistroHumor
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime DataHora { get; set; }

        public TipoHumor Humor { get; set; }
    }
}
