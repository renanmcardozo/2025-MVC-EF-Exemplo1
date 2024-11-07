using Microsoft.AspNetCore.Mvc;

namespace MVC_EF.Exemplo1.Controllers;

public class LivroController : Controller
{
    private readonly ApplicationDbContext _contexto;
    
    public LivroController(ApplicationDbContext contexto)
    {
        _contexto = contexto;
    }
    
    // GET
    public IActionResult Index()
    {
        var livros = _contexto.Livros;
        return View(livros);
    }
}