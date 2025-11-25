using CineReviewP2.Context;
using CineReviewP2.Models;
using CineReviewP2.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineReviewP2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public  ActionResult<IEnumerable<Usuario>> GetUsuarios()
        {
            return  _context.Usuarios
                .Include(u => u.Notas)
                .Include(u => u.Favoritos)
                .ToList();
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public  ActionResult<Usuario> GetUsuario(int id)
        {
            var usuario =  _context.Usuarios
                .Include(u => u.Notas)
                .Include(u => u.Favoritos)
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return usuario;
        }

        // POST: api/usuarios
        [HttpPost]
        public  ActionResult<Usuario> PostUsuario(UsuarioInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Email)) // valida se o email não é nulo ou vazio
            {
                return BadRequest(new { message = "Email é obrigatório" });
            }

            var usuario = new Usuario
            {
                Email = input.Email,
                Nome = input.Nome
            };

            _context.Usuarios.Add(usuario);
             _context.SaveChanges();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public  IActionResult PutUsuario(int id, UsuarioInputModel input)
        {
            var usuarioExistente =  _context.Usuarios.Find(id);
            if (usuarioExistente == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            usuarioExistente.Email = input.Email;
            usuarioExistente.Nome = input.Nome;

            _context.Usuarios.Update(usuarioExistente);
             _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public  IActionResult DeleteUsuario(int id)
        {
            var usuario =  _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            _context.Usuarios.Remove(usuario);
             _context.SaveChanges();

            return Ok(new { message = "Usuário deletado com sucesso" });
        }
    }
}
