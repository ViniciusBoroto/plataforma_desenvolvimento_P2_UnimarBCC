namespace CineReviewP2.InputModels
{
    public class UpdateMidiaInputModel
    {
        public string Nome { get; set; } = string.Empty;

        // Optional: only for Filme
        public int? DuracaoEmMinutos { get; set; }

        // Optional: only for Serie
        public int? Temporadas { get; set; }
    }
}
