namespace CineReviewP2.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string Nome { get; set; } = "";
        public ICollection<Nota> Notas { get; set; } 
        public ICollection<Favorito> Favoritos { get; set; } 
    }
}
    