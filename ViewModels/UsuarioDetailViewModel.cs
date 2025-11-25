namespace CineReviewP2.ViewModels
{
    public class UsuarioDetailViewModel : UsuarioViewModel
    {
        public List<NotaUsuarioViewModel> Notas { get; set; } = new();
        public List<FavoritoUsuarioViewModel> Favoritos { get; set; } = new();
    }
}
