using CineReviewP2.Context;
using CineReviewP2.Models;
using CineReviewP2.InputModels;
using CineReviewP2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
        public ActionResult<IEnumerable<UsuarioDetailViewModel>> GetUsuarios()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Notas)
                    .ThenInclude(n => n.Midia)
                .Include(u => u.Favoritos)
                    .ThenInclude(f => f.Midia)
                .ToList();

            return usuarios.Select(u => new UsuarioDetailViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Nome = u.Nome,
                Notas = u.Notas.Select(n => new NotaUsuarioViewModel
                {
                    Id = n.Id,
                    Valor = n.Valor,
                    Midia = new MidiaViewModel { Id = n.Midia.Id, Nome = n.Midia.Nome }
                }).ToList(),
                Favoritos = u.Favoritos.Select(f => new FavoritoUsuarioViewModel
                {
                    Id = f.Id,
                    Midia = new MidiaViewModel { Id = f.Midia.Id, Nome = f.Midia.Nome }
                }).ToList()
            }).ToList();
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public ActionResult<UsuarioDetailViewModel> GetUsuario(int id)
        {
            var usuario = _context.Usuarios
                .Include(u => u.Notas)
                    .ThenInclude(n => n.Midia)
                .Include(u => u.Favoritos)
                    .ThenInclude(f => f.Midia)
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            return new UsuarioDetailViewModel
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nome = usuario.Nome,
                Notas = usuario.Notas.Select(n => new NotaUsuarioViewModel
                {
                    Id = n.Id,
                    Valor = n.Valor,
                    Midia = new MidiaViewModel { Id = n.Midia.Id, Nome = n.Midia.Nome }
                }).ToList(),
                Favoritos = usuario.Favoritos.Select(f => new FavoritoUsuarioViewModel
                {
                    Id = f.Id,
                    Midia = new MidiaViewModel { Id = f.Midia.Id, Nome = f.Midia.Nome }
                }).ToList()
            };
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
        // GET: api/usuarios/ranking/avaliacoes
        [HttpGet("ranking/avaliacoes")]
        public ActionResult<IEnumerable<object>> GetRankingAvaliacoes()
        {
            var ranking = _context.Usuarios
                .Select(u => new
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    TotalAvaliacoes = u.Notas.Count
                })
                .OrderByDescending(u => u.TotalAvaliacoes)
                .ToList();

            return Ok(ranking);
        }

        // GET: api/usuarios/ranking/favoritos
        [HttpGet("ranking/favoritos")]
        public ActionResult<IEnumerable<object>> GetRankingFavoritos()
        {
            var ranking = _context.Usuarios
                .Select(u => new
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    TotalFavoritos = u.Favoritos.Count
                })
                .OrderByDescending(u => u.TotalFavoritos)
                .ToList();

            return Ok(ranking);
        }
    }
}
