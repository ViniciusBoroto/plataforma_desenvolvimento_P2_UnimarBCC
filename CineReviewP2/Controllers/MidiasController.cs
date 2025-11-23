using CineReviewP2.Context;
using CineReviewP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineReviewP2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MidiasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MidiasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/midias
        [HttpGet]
        public ActionResult<IEnumerable<Midia>> GetMidias()
        {
            return _context.Midias
                .Include(m => m.Notas)
                .Include(m => m.Favoritos)
                .ToList();
        }

        // GET: api/midias/5
        [HttpGet("{id}")]
        public ActionResult<Midia> GetMidia(int id)
        {
            var midia = _context.Midias
                .Include(m => m.Notas)
                .Include(m => m.Favoritos)
                .FirstOrDefault(m => m.Id == id);

            if (midia == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            return midia;
        }

        // POST: api/midias/filme
        [HttpPost("filme")]
        public ActionResult<Filme> PostFilme(Filme filme)
        {
            if (string.IsNullOrWhiteSpace(filme.Nome))
            {
                return BadRequest(new { message = "Nome é obrigatório" });
            }

            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction("GetMidia", new { id = filme.Id }, filme);
        }

        // POST: api/midias/serie
        [HttpPost("serie")]
        public ActionResult<Serie> PostSerie(Serie serie)
        {
            if (string.IsNullOrWhiteSpace(serie.Nome))
            {
                return BadRequest(new { message = "Nome é obrigatório" });
            }

            _context.Series.Add(serie);
            _context.SaveChanges();

            return CreatedAtAction("GetMidia", new { id = serie.Id }, serie);
        }

        // PUT: api/midias/5
        [HttpPut("{id}")]
        public IActionResult PutMidia(int id, Midia midia)
        {
            if (id != midia.Id)
            {
                return BadRequest(new { message = "ID não corresponde" });
            }

            var midiaExistente = _context.Midias.Find(id);
            if (midiaExistente == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            midiaExistente.Nome = midia.Nome;

            // Se for filme
            if (midiaExistente is Filme filmeExistente && midia is Filme filmeNovo)
            {
                filmeExistente.DuracaoEmMinutos = filmeNovo.DuracaoEmMinutos;
            }

            // Se for série
            if (midiaExistente is Serie serieExistente && midia is Serie serieNova)
            {
                serieExistente.Temporadas = serieNova.Temporadas;
            }

            _context.Midias.Update(midiaExistente);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/midias/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMidia(int id)
        {
            var midia = _context.Midias.Find(id);
            if (midia == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            _context.Midias.Remove(midia);
            _context.SaveChanges();

            return Ok(new { message = "Mídia deletada com sucesso" });
        }

        // GET: api/midias/filmes
        [HttpGet("filmes")]
        public ActionResult<IEnumerable<Filme>> GetFilmes()
        {
            return _context.Filmes
                .Include(f => f.Notas)
                .Include(f => f.Favoritos)
                .ToList();
        }

        // GET: api/midias/series
        [HttpGet("series")]
        public ActionResult<IEnumerable<Serie>> GetSeries()
        {
            return _context.Series
                .Include(s => s.Notas)
                .Include(s => s.Favoritos)
                .ToList();
        }
    }
}
