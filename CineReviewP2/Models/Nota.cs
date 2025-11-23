namespace CineReviewP2.Models
{
    public class Nota
    {
        public int Id { get; set; }
        public int Valor { get; set; }
        public Midia Midia { get; set; }
        public int MidiaId { get; set; }
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
    }
}
