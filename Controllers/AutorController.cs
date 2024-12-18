using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_EF.Exemplo1.Models;
using MVC_EF.Exemplo1;

namespace MVC_EF.Exemplo1.Controllers
{
    public class AutorController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public AutorController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autores.ToListAsync();
            return View("~/Views/Autor/Index.cshtml", autores);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
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
        
        public IActionResult Edit(int id)
        {
            var autor = _context.Autores.Find(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutorID, AutorNome, AutorDataNascimento, AutorEmail")] Autor autor)
        {
            if (id != autor.AutorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Autores.Any(e => e.AutorID == autor.AutorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

// Ação Delete para exibir a confirmação de exclusão
        public IActionResult Delete(int id)
        {
            var autor = _context.Autores.Find(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

// Ação Delete (POST) para realmente excluir o autor
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}