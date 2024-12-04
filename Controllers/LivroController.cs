using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_EF.Exemplo1.Models;

namespace MVC_EF.Exemplo1.Controllers;

public class LivroController : Controller
{
    private readonly ApplicationDbContext _contexto;
    
    public LivroController(ApplicationDbContext contexto)
    {
        _contexto = contexto;
    }

    // Se quiser injetar outros serviços, pode injetar um provedor de serviços, ao invés
    // do serviço específico
    // public LivroController(IServiceProvider serviceProvider)
    // {
    //     _contexto = serviceProvider.GetService<ApplicationDbContext>() ?? throw new InvalidOperationException(); 
    // }

    
    // GET
    public IActionResult Index()
    {
        var livros = _contexto.Livros
            .Include(p => p.EditoraDoLivro)
            .Include(p => p.AutoresDoLivro)
            .ThenInclude(p => p.Autor);

        var rset = new List<LivroEditoraAutorListViewModel>();
        foreach (var livro in livros)
        {
            var r = new LivroEditoraAutorListViewModel();
            r.Id = livro.LivroID;
            r.Titulo = livro.LivroTitulo;
            r.Ano = livro.LivroAnoPublicacao;
            r.Paginas = livro.LivroPaginas;
            r.ISBN = livro.LivroISBN;
            r.Editora = livro.EditoraDoLivro.EditoraNome;
            r.Autores = "";
            foreach (var autor in livro.AutoresDoLivro)
            {
                r.Autores = r.Autores + autor.Autor.AutorNome + ", ";
            }
            rset.Add(r);
        }
        
        return View(rset);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
            return RedirectToAction(nameof(Index));

        var livro = _contexto.Livros
            .Include(p => p.EditoraDoLivro)
            .Include(p => p.AutoresDoLivro)
            .ThenInclude(p => p.Autor)
            .FirstOrDefault(p=> p.LivroID == id.Value);
        if (livro == null)
        {
            return NotFound();
        }

        var livroViewModel = new LivroEditoraAutorEditViewModel();
        livroViewModel.LivroID = id.Value;
        livroViewModel.Titulo = livro.LivroTitulo;
        livroViewModel.Ano = livro.LivroAnoPublicacao;
        livroViewModel.Paginas = livro.LivroPaginas;
        livroViewModel.ISBN = livro.LivroISBN;
        livroViewModel.EditoraID = livro.EditoraID;
        livroViewModel.EditoraInputSelect = new SelectList(
            _contexto.Editoras.OrderBy(p => p.EditoraNome).ToList(),
            dataValueField: "EditoraID",
            dataTextField: "EditoraNome",
            selectedValue: livro.EditoraID);

        return View(livroViewModel);
    }

    [HttpPost, ActionName("Edit")]
    public IActionResult EditConfirm(int? id, LivroEditoraAutorEditViewModel livro)
    {
        if (id == null)
            return RedirectToAction(nameof(Index));

        if (id.Value != livro.LivroID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var livroViewModel = new LivroEditoraAutorEditViewModel();
            livroViewModel.LivroID = id.Value;
            livroViewModel.Titulo = livro.Titulo;
            livroViewModel.Ano = livro.Ano;
            livroViewModel.Paginas = livro.Paginas;
            livroViewModel.ISBN = livro.ISBN;
            livroViewModel.EditoraID = livro.EditoraID;
            livroViewModel.EditoraInputSelect = new SelectList(
                _contexto.Editoras.OrderBy(p => p.EditoraNome).ToList(),
                dataValueField: "EditoraID",
                dataTextField: "EditoraNome",
                selectedValue: livro.EditoraID);

            return View(livroViewModel);
        }

        var livroOriginal = _contexto.Livros
            .FirstOrDefault(p => p.LivroID == livro.LivroID);

        if (livroOriginal == null)
            return NotFound();

        if (livroOriginal.LivroPaginas != livro.Paginas)
            livroOriginal.LivroPaginas = livro.Paginas;
        if (livroOriginal.LivroTitulo != livro.Titulo)
            livroOriginal.LivroTitulo = livro.Titulo;
        if (livroOriginal.LivroAnoPublicacao != livro.Ano)
            livroOriginal.LivroAnoPublicacao = livro.Ano;
        if (livroOriginal.LivroISBN != livro.ISBN)
            livroOriginal.LivroISBN = livro.ISBN;

        if (livroOriginal.EditoraID != livro.EditoraID)
        {
            var editora = _contexto.Editoras.FirstOrDefault(p => p.EditoraID == livro.EditoraID);
            if (editora == null)
                return NotFound();
            livroOriginal.EditoraID = editora.EditoraID;
        }

        if (_contexto.Entry(livroOriginal).State == EntityState.Modified)
        {
            try
            {
                _contexto.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (!_contexto.Livros.Any(p => p.LivroID == livro.LivroID))
                    return NotFound();
                Console.WriteLine(e);
                throw;
            }
        }
        
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        var novo = new LivroEditoraAutorEditViewModel();
        novo.EditoraID = 0;
        novo.EditoraInputSelect = new SelectList(
            _contexto.Editoras.OrderBy(p => p.EditoraNome).ToList(),
            dataValueField: "EditoraID",
            dataTextField: "EditoraNome");

        return View(novo);
    }

    [HttpPost, ActionName("Create")]
    public IActionResult CreatePost(LivroEditoraAutorEditViewModel livro)
    {
        if (!ModelState.IsValid)
        {
            var novo = new LivroEditoraAutorEditViewModel();
            novo.Titulo = livro.Titulo;
            novo.Ano = livro.Ano;
            novo.Paginas = livro.Paginas;
            novo.ISBN = livro.ISBN;
            novo.EditoraID = livro.EditoraID;
            novo.EditoraInputSelect = new SelectList(
                _contexto.Editoras.OrderBy(p => p.EditoraNome).ToList(),
                dataValueField: "EditoraID",
                dataTextField: "EditoraNome",
                selectedValue: livro.EditoraID);

            return View(novo);
        }

        var novoLivro = new Livro();
        novoLivro.LivroTitulo = livro.Titulo;
        novoLivro.LivroPaginas = livro.Paginas;
        novoLivro.LivroAnoPublicacao = livro.Ano;
        novoLivro.LivroISBN = livro.ISBN;
        var editora = _contexto.Editoras.FirstOrDefault(p => p.EditoraID == livro.EditoraID);
        if (editora == null)
            return NotFound();
        novoLivro.EditoraDoLivro = editora;

        _contexto.Livros.Add(novoLivro);
        
        try
        {
            _contexto.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
            return RedirectToAction(nameof(Index));
        
        var livro = _contexto.Livros
            .Include(p => p.EditoraDoLivro)
            .Include(p => p.AutoresDoLivro)
            .ThenInclude(p => p.Autor)
            .FirstOrDefault(p=> p.LivroID == id.Value);
        if (livro == null)
        {
            return NotFound();
        }

        var objeto = new LivroEditoraAutorListViewModel
        {
            Id = livro.LivroID,
            Ano = livro.LivroAnoPublicacao,
            Editora = livro.EditoraDoLivro.EditoraNome,
            ISBN = livro.LivroISBN,
            Paginas = livro.LivroPaginas,
            Titulo = livro.LivroTitulo
        };
        objeto.Autores = "";
        foreach (var autor in livro.AutoresDoLivro)
        {
            objeto.Autores = objeto.Autores + autor.Autor.AutorNome + ", ";
        }

        return View(objeto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        if (id == null)
            return RedirectToAction(nameof(Index));
        
        var livro = _contexto.Livros
            .FirstOrDefault(p=> p.LivroID == id.Value);
        if (livro == null)
        {
            return NotFound();
        }

        _contexto.Livros.Remove(livro);
        
        try
        {
            _contexto.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

}
