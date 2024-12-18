using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_EF.Exemplo1.Models;

namespace MVC_EF.Exemplo1.Controllers
{
    public class AutorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autores.ToListAsync();
            return View("~/Views/Autor/Index.cshtml", autores);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorNome,AutorDataNascimento,AutorEmail")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var autor = _context.Autores.Find(id);
            if (autor == null)
            {
                return NotFound();
            }

            // Convertendo para o ViewModel
            var viewModel = new AutorEditViewModel
            {
                AutorID = autor.AutorID,
                AutorNome = autor.AutorNome,
                AutorDataNascimento = autor.AutorDataNascimento, // Manteve como DateOnly
                AutorEmail = autor.AutorEmail
            };

            return View(viewModel);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AutorEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var autorAtualizar = _context.Autores.Find(viewModel.AutorID);
            if (autorAtualizar == null)
            {
                return NotFound();
            }

            // Atualizando os campos
            autorAtualizar.AutorNome = viewModel.AutorNome;
            autorAtualizar.AutorDataNascimento = viewModel.AutorDataNascimento; // Manteve como DateOnly
            autorAtualizar.AutorEmail = viewModel.AutorEmail;

            try
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Erro ao editar o autor.");
                Console.WriteLine(e);
                return View(viewModel);
            }
        }

        // DELETE GET
        public IActionResult Delete(int id)
        {
            var autor = _context.Autores.Find(id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var autor = _context.Autores.Find(id);
            if (autor == null)
            {
                return NotFound();
            }

            try
            {
                _context.Autores.Remove(autor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Erro ao deletar o autor.");
                Console.WriteLine(e);
                return View(autor);
            }
        }
    }
}
