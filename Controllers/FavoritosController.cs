using CineReviewP2.Context;
using CineReviewP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineReviewP2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/favoritos
        [HttpGet]
        public ActionResult<IEnumerable<Favorito>> GetFavoritos()
        {
            return _context.Favoritos
                .Include(f => f.Usuario)
                .Include(f => f.Midia)
                .ToList();
        }

        // GET: api/favoritos/5
        [HttpGet("{id}")]
        public ActionResult<Favorito> GetFavorito(int id)
        {
            var favorito = _context.Favoritos
                .Include(f => f.Usuario)
                .Include(f => f.Midia)
                .FirstOrDefault(f => f.Id == id);

            if (favorito == null)
            {
                return NotFound(new { message = "Favorito não encontrado" });
            }

            return favorito;
        }

        // GET: api/favoritos/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public ActionResult<IEnumerable<Favorito>> GetFavoritosPorUsuario(int usuarioId)
        {
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return _context.Favoritos
                .Where(f => f.UsuarioId == usuarioId)
                .Include(f => f.Midia)
                .ToList();
        }

        // GET: api/favoritos/midia/5
        [HttpGet("midia/{midiaId}")]
        public ActionResult<IEnumerable<Favorito>> GetFavoritosPorMidia(int midiaId)
        {
            var midia = _context.Midias.Find(midiaId);
            if (midia == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            return _context.Favoritos
                .Where(f => f.MidiaId == midiaId)
                .Include(f => f.Usuario)
                .ToList();
        }

        // POST: api/favoritos
        [HttpPost]
        public ActionResult<Favorito> PostFavorito(Favorito favorito)
        {
            // Validar se usuário existe
            var usuarioExiste = _context.Usuarios.Any(u => u.Id == favorito.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest(new { message = "Usuário não encontrado" });
            }

            // Validar se mídia existe
            var midiaExiste = _context.Midias.Any(m => m.Id == favorito.MidiaId);
            if (!midiaExiste)
            {
                return BadRequest(new { message = "Mídia não encontrada" });
            }

            // Verificar se já não é favorito
            var jaEhFavorito = _context.Favoritos
                .Any(f => f.UsuarioId == favorito.UsuarioId && f.MidiaId == favorito.MidiaId);
            if (jaEhFavorito)
            {
                return BadRequest(new { message = "Essa mídia já é favorita deste usuário" });
            }

            _context.Favoritos.Add(favorito);
            _context.SaveChanges();

            return CreatedAtAction("GetFavorito", new { id = favorito.Id }, favorito);
        }

        // DELETE: api/favoritos/5
        [HttpDelete("{id}")]
        public IActionResult DeleteFavorito(int id)
        {
            var favorito = _context.Favoritos.Find(id);
            if (favorito == null)
            {
                return NotFound(new { message = "Favorito não encontrado" });
            }

            _context.Favoritos.Remove(favorito);
            _context.SaveChanges();

            return Ok(new { message = "Favorito removido com sucesso" });
        }

        // DELETE: api/favoritos/usuario/5/midia/5
        [HttpDelete("usuario/{usuarioId}/midia/{midiaId}")]
        public IActionResult DeleteFavoritoByUserAndMedia(int usuarioId, int midiaId)
        {
            var favorito = _context.Favoritos
                .FirstOrDefault(f => f.UsuarioId == usuarioId && f.MidiaId == midiaId);

            if (favorito == null)
            {
                return NotFound(new { message = "Favorito não encontrado" });
            }

            _context.Favoritos.Remove(favorito);
            _context.SaveChanges();

            return Ok(new { message = "Favorito removido com sucesso" });
        }
    }
}
