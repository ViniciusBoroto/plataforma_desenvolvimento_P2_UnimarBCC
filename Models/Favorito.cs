namespace CineReviewP2.Models
{
    public class Favorito
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
        public Midia Midia { get; set; }
        public int MidiaId { get; set; }
    }
}
