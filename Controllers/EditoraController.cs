using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_EF.Exemplo1.Models;

namespace MVC_EF.Exemplo1.Controllers
{
    public class EditoraController : Controller
    {
        private readonly ApplicationDbContext _contexto;
        private readonly int _pagesize;

        public EditoraController(ApplicationDbContext contexto, IConfiguration configuration)
        {
            _contexto = contexto;
            var pagesize = Convert.ToInt32(configuration.
                GetSection("ViewOptions").
                GetSection("PageSize").Value);
            _pagesize = pagesize > 0 ? pagesize : 10;
        }

        // Método Index: Lista as editoras
        public IActionResult Index()
        {
            var editoras = _contexto.Editoras.ToList();
            return View(editoras);
        }

        public IActionResult Create()
        {
            var novo = new EditoraEditViewModel();
            return View(novo);
        }

        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost(EditoraEditViewModel editora)
        {
            if (!ModelState.IsValid)
            {
                return View(editora);
            }

            var novaEditora = new Editora
            {
                EditoraNome = editora.EditoraNome,
                EditoraLogradouro = editora.EditoraLogradouro,
                EditoraNumero = editora.EditoraNumero,
                EditoraComplemento = editora.EditoraComplemento,
                EditoraCidade = editora.EditoraCidade,
                EditoraUF = editora.EditoraUF,
                EditoraPais = editora.EditoraPais,
                EditoraCEP = editora.EditoraCEP,
                EditoraTelefone = editora.EditoraTelefone
            };

            try
            {
                _contexto.Editoras.Add(novaEditora);
                _contexto.SaveChanges();
                return RedirectToAction(nameof(Index)); // Redireciona para a lista após salvar
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Erro ao salvar a editora. Tente novamente.");
                Console.WriteLine(e);
                return View(editora);
            }
        }
    }
}