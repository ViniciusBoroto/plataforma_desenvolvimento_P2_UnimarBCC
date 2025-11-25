using CineReviewP2.Context;
using CineReviewP2.Models;
using CineReviewP2.InputModels;
using CineReviewP2.ViewModels;
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
        public ActionResult<IEnumerable<MidiaViewModel>> GetMidias()
        {
            var midias = _context.Midias.ToList();
            
            return midias.Select(m => new MidiaViewModel
            {
                Id = m.Id,
                Nome = m.Nome
            }).ToList();
        }

        // GET: api/midias/5
        [HttpGet("{id}")]
        public ActionResult<MidiaViewModel> GetMidia(int id)
        {
            var midia = _context.Midias
                .Include(m => m.Notas)
                    .ThenInclude(n => n.Usuario)
                .Include(m => m.Favoritos)
                    .ThenInclude(f => f.Usuario)
                .FirstOrDefault(m => m.Id == id);

            if (midia == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            if (midia is Filme filme)
            {
                return new FilmeViewModel
                {
                    Id = filme.Id,
                    Nome = filme.Nome,
                    DuracaoEmMinutos = filme.DuracaoEmMinutos,
                    Notas = filme.Notas.Select(n => new NotaSimplesViewModel
                    {
                        Id = n.Id,
                        Valor = n.Valor,
                        Usuario = new UsuarioViewModel { Id = n.Usuario.Id, Email = n.Usuario.Email, Nome = n.Usuario.Nome }
                    }).ToList(),
                    Favoritos = filme.Favoritos.Select(f => new FavoritoSimplesViewModel
                    {
                        Id = f.Id,
                        Usuario = new UsuarioViewModel { Id = f.Usuario.Id, Email = f.Usuario.Email, Nome = f.Usuario.Nome }
                    }).ToList()
                };
            }
            else if (midia is Serie serie)
            {
                return new SerieViewModel
                {
                    Id = serie.Id,
                    Nome = serie.Nome,
                    Temporadas = serie.Temporadas,
                    Notas = serie.Notas.Select(n => new NotaSimplesViewModel
                    {
                        Id = n.Id,
                        Valor = n.Valor,
                        Usuario = new UsuarioViewModel { Id = n.Usuario.Id, Email = n.Usuario.Email, Nome = n.Usuario.Nome }
                    }).ToList(),
                    Favoritos = serie.Favoritos.Select(f => new FavoritoSimplesViewModel
                    {
                        Id = f.Id,
                        Usuario = new UsuarioViewModel { Id = f.Usuario.Id, Email = f.Usuario.Email, Nome = f.Usuario.Nome }
                    }).ToList()
                };
            }

            return new MidiaViewModel { Id = midia.Id, Nome = midia.Nome };
        }

        // POST: api/midias/filme
        [HttpPost("filme")]
        public ActionResult<Filme> PostFilme(FilmeInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Nome))
            {
                return BadRequest(new { message = "Nome é obrigatório" });
            }

            var filme = new Filme
            {
                Nome = input.Nome,
                DuracaoEmMinutos = input.DuracaoEmMinutos
            };

            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction("GetMidia", new { id = filme.Id }, filme);
        }

        // POST: api/midias/serie
        [HttpPost("serie")]
        public ActionResult<Serie> PostSerie(SerieInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Nome))
            {
                return BadRequest(new { message = "Nome é obrigatório" });
            }

            var serie = new Serie
            {
                Nome = input.Nome,
                Temporadas = input.Temporadas
            };

            _context.Series.Add(serie);
            _context.SaveChanges();

            return CreatedAtAction("GetMidia", new { id = serie.Id }, serie);
        }

        // PUT: api/midias/5
        [HttpPut("{id}")]
        public IActionResult PutMidia(int id, UpdateMidiaInputModel input)
        {
            var midiaExistente = _context.Midias.Find(id);
            if (midiaExistente == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            midiaExistente.Nome = input.Nome;

            // Se for filme
            if (midiaExistente is Filme filmeExistente && input.DuracaoEmMinutos.HasValue)
            {
                filmeExistente.DuracaoEmMinutos = input.DuracaoEmMinutos.Value;
            }

            // Se for série
            if (midiaExistente is Serie serieExistente && input.Temporadas.HasValue)
            {
                serieExistente.Temporadas = input.Temporadas.Value;
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
        public ActionResult<IEnumerable<FilmeViewModel>> GetFilmes()
        {
            var filmes = _context.Filmes
                .Include(f => f.Notas)
                    .ThenInclude(n => n.Usuario)
                .Include(f => f.Favoritos)
                    .ThenInclude(f => f.Usuario)
                .ToList();

            return filmes.Select(f => new FilmeViewModel
            {
                Id = f.Id,
                Nome = f.Nome,
                DuracaoEmMinutos = f.DuracaoEmMinutos,
                Notas = f.Notas.Select(n => new NotaSimplesViewModel
                {
                    Id = n.Id,
                    Valor = n.Valor,
                    Usuario = new UsuarioViewModel { Id = n.Usuario.Id, Email = n.Usuario.Email, Nome = n.Usuario.Nome }
                }).ToList(),
                Favoritos = f.Favoritos.Select(fav => new FavoritoSimplesViewModel
                {
                    Id = fav.Id,
                    Usuario = new UsuarioViewModel { Id = fav.Usuario.Id, Email = fav.Usuario.Email, Nome = fav.Usuario.Nome }
                }).ToList()
            }).ToList();
        }

        // GET: api/midias/series
        [HttpGet("series")]
        public ActionResult<IEnumerable<SerieViewModel>> GetSeries()
        {
            var series = _context.Series
                .Include(s => s.Notas)
                    .ThenInclude(n => n.Usuario)
                .Include(s => s.Favoritos)
                    .ThenInclude(f => f.Usuario)
                .ToList();

            return series.Select(s => new SerieViewModel
            {
                Id = s.Id,
                Nome = s.Nome,
                Temporadas = s.Temporadas,
                Notas = s.Notas.Select(n => new NotaSimplesViewModel
                {
                    Id = n.Id,
                    Valor = n.Valor,
                    Usuario = new UsuarioViewModel { Id = n.Usuario.Id, Email = n.Usuario.Email, Nome = n.Usuario.Nome }
                }).ToList(),
                Favoritos = s.Favoritos.Select(fav => new FavoritoSimplesViewModel
                {
                    Id = fav.Id,
                    Usuario = new UsuarioViewModel { Id = fav.Usuario.Id, Email = fav.Usuario.Email, Nome = fav.Usuario.Nome }
                }).ToList()
            }).ToList();
        }
    }
}
