namespace CineReviewP2.ViewModels
{
    public class FilmeViewModel : MidiaViewModel
    {
        public int DuracaoEmMinutos { get; set; }
        public List<NotaSimplesViewModel> Notas { get; set; } = new();
        public List<FavoritoSimplesViewModel> Favoritos { get; set; } = new();
    }
}
