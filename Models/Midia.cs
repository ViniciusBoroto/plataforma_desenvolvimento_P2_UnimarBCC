namespace CineReviewP2.Models
{
    public class Midia
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public ICollection<Nota> Notas { get; set; } 
        public ICollection<Favorito> Favoritos { get; set; } 
    }
}
