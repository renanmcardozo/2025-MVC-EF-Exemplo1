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
        var livro = _contexto.Livros
            .Include(p => p.EditoraDoLivro)
            .Include(p => p.AutoresDoLivro)
            .ThenInclude(p => p.Autor)
            .FirstOrDefault(p=> p.LivroID == id);
        if (livro == null)
        {
            return NotFound();
        }

        var livroViewModel = new LivroEditoraAutorEditViewModel();
        livroViewModel.LivroID = livro.LivroID;
        livroViewModel.Titulo = livro.LivroTitulo;
        livroViewModel.Ano = livro.LivroAnoPublicacao;
        livroViewModel.Paginas = livro.LivroPaginas;
        livroViewModel.ISBN = livro.LivroISBN;
        livroViewModel.EditoraID = livro.EditoraID;
        livroViewModel.EditoraInputSelect = new SelectList(
            _contexto.Editoras.OrderBy(p => p.EditoraNome).ToList(),
            dataValueField: "EditoraID",
            dataTextField: "EditoraNome");

        return View(livroViewModel);
    }

    public IActionResult EditConfirm(int? id, LivroEditoraAutorEditViewModel livro)
    {
        if (id != livro.LivroID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var livroViewModel = new LivroEditoraAutorEditViewModel();
            livroViewModel.LivroID = livro.LivroID;
            livroViewModel.Titulo = livro.Titulo;
            livroViewModel.Ano = livro.Ano;
            livroViewModel.Paginas = livro.Paginas;
            livroViewModel.ISBN = livro.ISBN;
            livroViewModel.EditoraID = livro.EditoraID;
            livroViewModel.EditoraInputSelect = new SelectList(
                _contexto.Editoras.OrderBy(p => p.EditoraNome).ToList(),
                dataValueField: "EditoraID",
                dataTextField: "EditoraNome");

            return View("Edit", livroViewModel);
        }

        // Alterar os dados
        return Redirect($"Livro/Index/");
    }
}