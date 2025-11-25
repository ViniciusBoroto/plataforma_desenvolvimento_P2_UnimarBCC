using CineReviewP2.Context;
using CineReviewP2.Models;
using CineReviewP2.InputModels;
using CineReviewP2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        public ActionResult<IEnumerable<NotaViewModel>> GetNotas()
        {
            var notas = _context.Notas
                .Include(n => n.Usuario)
                .Include(n => n.Midia)
                .ToList();

            var viewModels = notas.Select(n => new NotaViewModel
            {
                Id = n.Id,
                Valor = n.Valor,
                Usuario = new UsuarioViewModel
                {
                    Id = n.Usuario.Id,
                    Email = n.Usuario.Email,
                    Nome = n.Usuario.Nome
                },
                Midia = new MidiaViewModel
                {
                    Id = n.Midia.Id,
                    Nome = n.Midia.Nome
                }
            }).ToList();

            return viewModels;
        }

        // GET: api/notas/5
        [HttpGet("{id}")]
        public ActionResult<NotaViewModel> GetNota(int id)
        {
            var nota = _context.Notas
                .Include(n => n.Usuario)
                .Include(n => n.Midia)
                .FirstOrDefault(n => n.Id == id);

            if (nota == null)
            {
                return NotFound(new { message = "Nota não encontrada" });
            }

            var viewModel = new NotaViewModel
            {
                Id = nota.Id,
                Valor = nota.Valor,
                Usuario = new UsuarioViewModel
                {
                    Id = nota.Usuario.Id,
                    Email = nota.Usuario.Email,
                    Nome = nota.Usuario.Nome
                },
                Midia = new MidiaViewModel
                {
                    Id = nota.Midia.Id,
                    Nome = nota.Midia.Nome
                }
            };

            return viewModel;
        }

        // GET: api/notas/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public ActionResult<IEnumerable<NotaViewModel>> GetNotasPorUsuario(int usuarioId)
        {
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            var notas = _context.Notas
                .Where(n => n.UsuarioId == usuarioId)
                .Include(n => n.Midia)
                .Include(n => n.Usuario) 
                .ToList();

            var viewModels = notas.Select(n => new NotaViewModel
            {
                Id = n.Id,
                Valor = n.Valor,
                Usuario = new UsuarioViewModel
                {
                    Id = n.Usuario.Id,
                    Email = n.Usuario.Email,
                    Nome = n.Usuario.Nome
                },
                Midia = new MidiaViewModel
                {
                    Id = n.Midia.Id,
                    Nome = n.Midia.Nome
                }
            }).ToList();

            return viewModels;
        }

        // GET: api/notas/midia/5
        [HttpGet("midia/{midiaId}")]
        public ActionResult<IEnumerable<NotaViewModel>> GetNotasPorMidia(int midiaId)
        {
            var midia = _context.Midias.Find(midiaId);
            if (midia == null)
            {
                return NotFound(new { message = "Mídia não encontrada" });
            }

            var notas = _context.Notas
                .Where(n => n.MidiaId == midiaId)
                .Include(n => n.Usuario)
                .Include(n => n.Midia) 
                .ToList();

            var viewModels = notas.Select(n => new NotaViewModel
            {
                Id = n.Id,
                Valor = n.Valor,
                Usuario = new UsuarioViewModel
                {
                    Id = n.Usuario.Id,
                    Email = n.Usuario.Email,
                    Nome = n.Usuario.Nome
                },
                Midia = new MidiaViewModel
                {
                    Id = n.Midia.Id,
                    Nome = n.Midia.Nome
                }
            }).ToList();

            return viewModels;
        }

        // POST: api/notas
        [HttpPost]
        public  ActionResult<Nota> PostNota(NotaInputModel input)
        {
            // Validar se usuário existe
            var usuarioExiste =  _context.Usuarios.Any(u => u.Id == input.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest(new { message = "Usuário não encontrado" });
            }

            // Validar se mídia existe
            var midiaExiste =  _context.Midias.Any(m => m.Id == input.MidiaId);
            if (!midiaExiste)
            {
                return BadRequest(new { message = "Mídia não encontrada" });
            }

            // Validar valor da nota (0-10)
            if (input.Valor < 0 || input.Valor > 10)
            {
                return BadRequest(new { message = "Valor da nota deve estar entre 0 e 10" });
            }

            var nota = new Nota
            {
                Valor = input.Valor,
                UsuarioId = input.UsuarioId,
                MidiaId = input.MidiaId
            };

            _context.Notas.Add(nota);
             _context.SaveChanges();

            return CreatedAtAction("GetNota", new { id = nota.Id }, nota);
        }

        // PUT: api/notas/5
        [HttpPut("{id}")]
        public  IActionResult PutNota(int id, NotaInputModel input)
        {
            var notaExistente =  _context.Notas.Find(id);
            if (notaExistente == null)
            {
                return NotFound(new { message = "Nota não encontrada" });
            }

            // Validar valor da nota (0-10)
            if (input.Valor < 0 || input.Valor > 10)
            {
                return BadRequest(new { message = "Valor da nota deve estar entre 0 e 10" });
            }

            notaExistente.Valor = input.Valor;
            
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
