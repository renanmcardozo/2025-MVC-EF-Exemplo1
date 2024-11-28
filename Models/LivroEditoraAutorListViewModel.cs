using System.ComponentModel.DataAnnotations;

namespace MVC_EF.Exemplo1.Models;

public class LivroEditoraAutorListViewModel
{
    public int Id { get; set; }

    [Display (Name = "Título do livro")]
    public string Titulo { get; set; }
    [Display (Name = "Número de páginas")]
    public ushort Paginas { get; set; }
    [Display (Name = "Ano de publicação")]
    public ushort Ano { get; set; }
    [Display (Name = "ISBN-13")]
    public string ISBN { get; set; }
    [Display (Name = "Editora")]
    public string Editora { get; set; }
    [Display (Name = "Lista de autores")]
    public string Autores { get; set; }
}