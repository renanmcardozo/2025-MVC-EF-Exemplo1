using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MVC_EF.Exemplo1.Models;
using X.PagedList.Extensions;

namespace MVC_EF.Exemplo1.Controllers;

public class LivroController : Controller
{
    private readonly ApplicationDbContext _contexto;
    private readonly int _pagesize;
    
    public LivroController(ApplicationDbContext contexto, IConfiguration configuration)
    {
        _contexto = contexto;
        var pagesize = Convert.ToInt32(configuration.
            GetSection("ViewOptions").
            GetSection("PageSize").Value);
        _pagesize = pagesize > 0 ? pagesize : 10;
    }
    
    // GET
    public IActionResult Index(string? keyword, int? pagina)
    {
        IQueryable<Livro> livros = _contexto.Livros;
        if (!string.IsNullOrEmpty(keyword))
        {
            livros = livros
                .Include(p => p.EditoraDoLivro)
                .Include(p => p.AutoresDoLivro)
                .ThenInclude(p => p.Autor)
                .Where(p => p.LivroTitulo.ToUpper().Contains(keyword.ToUpper()))
                .OrderBy(p => p.LivroTitulo);
            ViewBag.keyword = keyword;
        }
        else
        {
            livros = _contexto.Livros
                .Include(p => p.EditoraDoLivro)
                .Include(p => p.AutoresDoLivro)
                .ThenInclude(p => p.Autor)
                .OrderBy(p => p.LivroTitulo);
            ViewBag.keyword = "";
        }

        var rset = livros.Select(livro => new LivroEditoraAutorListViewModel
            {
                Ano = livro.LivroAnoPublicacao,
                Editora = livro.EditoraDoLivro.EditoraNome,
                ISBN = livro.LivroISBN,
                Paginas = livro.LivroPaginas,
                Titulo = livro.LivroTitulo,
                Autores = livro.AutoresDoLivro.Any() == true ?
                    string.Join(", ", livro.AutoresDoLivro.Select(a => a.Autor.AutorNome))
                    : "Sem autores definidos",
                Id = livro.LivroID
            }).
            ToPagedList(pagina ?? 1, _pagesize);

        ViewBag.primeiro = rset.FirstItemOnPage;
        ViewBag.ultimo = rset.LastItemOnPage;
        ViewBag.total = rset.TotalItemCount;
        
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
            var livroViewModel = new LivroEditoraAutorEditViewModel
            {
                LivroID = id.Value,
                Titulo = livro.Titulo,
                Ano = livro.Ano,
                Paginas = livro.Paginas,
                ISBN = livro.ISBN,
                EditoraID = livro.EditoraID,
                EditoraInputSelect = new SelectList(
                    _contexto.Editoras.OrderBy(p => p.EditoraNome).ToList(),
                    dataValueField: "EditoraID",
                    dataTextField: "EditoraNome",
                    selectedValue: livro.EditoraID)
            };

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

    public IActionResult AssociarAutor(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var livro = _contexto.Livros
            .Include(p => p.AutoresDoLivro)
            .ThenInclude(p => p.Autor)
            .FirstOrDefault(p => p.LivroID == id);
        if (livro == null)
        {
            return NotFound();
        }

        var autores = _contexto.Autores
            .OrderBy(a => a.AutorNome)
            .ToList();

        var viewModel = new AdicionarAutorLivroViewModel
        {
            LivroID = livro.LivroID,
            LivroTitulo = livro.LivroTitulo,
            Autores = autores,
            AutoresSelecionados = livro.AutoresDoLivro
                .Select(a => a.AutorID)
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult AssociarAutor(AdicionarAutorLivroViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var autoresAtuais = _contexto.AutoresLivro
                .Where(l => l.LivroID == modelo.LivroID)
                .ToList();

            _contexto.AutoresLivro.RemoveRange(autoresAtuais);
            _contexto.SaveChanges();

            if (modelo.AutoresSelecionados != null)
            {
                ushort contador = 0;
                foreach (var autor in modelo.AutoresSelecionados)
                {
                    _contexto.AutoresLivro.Add(new AutorLivro
                    {
                        LivroID = modelo.LivroID,
                        AutorID = autor,
                        OrdemAutoria = contador
                    });
                    contador += 1;
                }

            }

            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(modelo);
    }

}
