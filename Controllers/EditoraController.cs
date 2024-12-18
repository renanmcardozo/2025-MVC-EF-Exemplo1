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

        // CREATE GET
        public IActionResult Create()
        {
            var novo = new EditoraEditViewModel();
            return View(novo);
        }

        // CREATE POST
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

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var editora = _contexto.Editoras.Find(id);
            if (editora == null)
            {
                return NotFound();
            }

            var viewModel = new EditoraEditViewModel
            {
                EditoraID = editora.EditoraID,
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

            return View(viewModel);
        }

        // EDIT POST
        [HttpPost]
        public IActionResult Edit(EditoraEditViewModel editora)
        {
            if (!ModelState.IsValid)
            {
                return View(editora);
            }

            var editoraAtualizar = _contexto.Editoras.Find(editora.EditoraID);
            if (editoraAtualizar == null)
            {
                return NotFound();
            }

            // Atualizando os campos
            editoraAtualizar.EditoraNome = editora.EditoraNome;
            editoraAtualizar.EditoraLogradouro = editora.EditoraLogradouro;
            editoraAtualizar.EditoraNumero = editora.EditoraNumero;
            editoraAtualizar.EditoraCidade = editora.EditoraCidade;
            editoraAtualizar.EditoraUF = editora.EditoraUF;
            editoraAtualizar.EditoraPais = editora.EditoraPais;
            editoraAtualizar.EditoraCEP = editora.EditoraCEP;
            editoraAtualizar.EditoraTelefone = editora.EditoraTelefone;

            try
            {
                _contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Erro ao editar a editora.");
                Console.WriteLine(e);
                return View(editora);
            }
        }

        // DELETE GET
        public IActionResult Delete(int id)
        {
            var editora = _contexto.Editoras.Find(id);
            if (editora == null)
            {
                return NotFound();
            }

            return View(editora);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var editora = _contexto.Editoras.Find(id);
            if (editora == null)
            {
                return NotFound();
            }

            try
            {
                _contexto.Editoras.Remove(editora);
                _contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Erro ao deletar a editora.");
                Console.WriteLine(e);
                return View(editora);
            }
        }
    }
}
