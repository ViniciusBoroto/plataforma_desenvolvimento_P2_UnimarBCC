namespace CineReviewP2.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string Nome { get; set; } = "";
        public List<Nota> Notas { get; set; } = new();
        public List<Favorito> Favoritos { get; set; } = new();
    }
}
