namespace MVC_EF.Exemplo1.Models;

public class AdicionarAutorLivroViewModel
{
    public int LivroID { get; set; }
    public string? LivroTitulo { get; set; }
    public List<Autor>? Autores { get; set; }
    public List<int>? AutoresSelecionados { get; set; }
}
