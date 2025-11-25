namespace CineReviewP2.ViewModels
{
    public class NotaViewModel
    {
        public int Id { get; set; }
        public int Valor { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public MidiaViewModel Midia { get; set; }
    }
}
