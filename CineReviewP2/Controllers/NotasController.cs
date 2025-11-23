using CineReviewP2.Context;
using CineReviewP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineReviewP2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/notas
        [HttpGet]
        public  ActionResult<IEnumerable<Nota>> GetNotas()
        {
            return  _context.Notas
                .Include(n => n.Usuario)
                .Include(n => n.Midia)
                .ToList();
        }

        // GET: api/notas/5
        [HttpGet("{id}")]
        public  ActionResult<Nota> GetNota(int id)
        {
            var nota =  _context.Notas
                .Include(n => n.Usuario)
                .Include(n => n.Midia)
                .FirstOrDefault(n => n.Id == id);

            if (nota == null)
            {
                return NotFound(new { message = "Nota não encontrada" });
            }

            return nota;
        }

        // GET: api/notas/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public  ActionResult<IEnumerable<Nota>> GetNotasPorUsuario(int usuarioId)
        {
            var usuario =  _context.Usuarios.Find(usuarioId);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return  _context.Notas
                .Where(n => n.UsuarioId == usuarioId)
                .Include(n => n.Midia)
                .ToList();
        }

        // GET: api/notas/midia/5
        [HttpGet("midia/{midiaId}")]
        public  ActionResult<IEnumerable<Nota>> GetNotasPorMidia(int midiaId)
        {
            var midia =  _context.Midias.Find(midiaId);
            if (midia == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            return  _context.Notas
                .Where(n => n.MidiaId == midiaId)
                .Include(n => n.Usuario)
                .ToList();
        }

        // POST: api/notas
        [HttpPost]
        public  ActionResult<Nota> PostNota(Nota nota)
        {
            // Validar se usuário existe
            var usuarioExiste =  _context.Usuarios.Any(u => u.Id == nota.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest(new { message = "Usuário não encontrado" });
            }

            // Validar se mídia existe
            var midiaExiste =  _context.Midias.Any(m => m.Id == nota.MidiaId);
            if (!midiaExiste)
            {
                return BadRequest(new { message = "Mídia não encontrada" });
            }

            // Validar valor da nota (0-10)
            if (nota.Valor < 0 || nota.Valor > 10)
            {
                return BadRequest(new { message = "Valor da nota deve estar entre 0 e 10" });
            }

            _context.Notas.Add(nota);
             _context.SaveChanges();

            return CreatedAtAction("GetNota", new { id = nota.Id }, nota);
        }

        // PUT: api/notas/5
        [HttpPut("{id}")]
        public  IActionResult PutNota(int id, Nota nota)
        {
            if (id != nota.Id)
            {
                return BadRequest(new { message = "ID não corresponde" });
            }

            var notaExistente =  _context.Notas.Find(id);
            if (notaExistente == null)
            {
                return NotFound(new { message = "Nota não encontrada" });
            }

            // Validar valor da nota (0-10)
            if (nota.Valor < 0 || nota.Valor > 10)
            {
                return BadRequest(new { message = "Valor da nota deve estar entre 0 e 10" });
            }

            notaExistente.Valor = nota.Valor;

            _context.Notas.Update(notaExistente);
             _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/notas/5
        [HttpDelete("{id}")]
        public  IActionResult DeleteNota(int id)
        {
            var nota =  _context.Notas.Find(id);
            if (nota == null)
            {
                return NotFound(new { message = "Nota não encontrada" });
            }

            _context.Notas.Remove(nota);
             _context.SaveChanges();

            return Ok(new { message = "Nota deletada com sucesso" });
        }
    }
}
