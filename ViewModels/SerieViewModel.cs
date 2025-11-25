namespace CineReviewP2.ViewModels
{
    public class SerieViewModel : MidiaViewModel
    {
        public int Temporadas { get; set; }
        public List<NotaSimplesViewModel> Notas { get; set; } = new();
        public List<FavoritoSimplesViewModel> Favoritos { get; set; } = new();
    }
}
