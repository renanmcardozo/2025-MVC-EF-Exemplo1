using Microsoft.AspNetCore.Mvc;

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
        var livros = _contexto.Livros;
        return View();
    }
}