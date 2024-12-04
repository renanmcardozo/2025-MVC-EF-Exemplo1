using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_EF.Exemplo1.Models;

public class LivroEditoraAutorEditViewModel
{
    public int LivroID { get; set; }
    [Display (Name = "Título do livro")]
    [Required(ErrorMessage = "O título do livro é obrigatório")]
    [StringLength(120, ErrorMessage = "O título deve ter até 120 caracteres")]
    public string Titulo { get; set; }
    
    [Display (Name = "Número de páginas")]
    [Required(ErrorMessage = "É obrigatório indicar o número de páginas")]
    [Range(1, maximum: 5000, ErrorMessage = "O livro deve ter entre 1 e 5000 páginas")]
    public ushort Paginas { get; set; }
    
    [Display (Name = "Ano de publicação")]
    [Required(ErrorMessage = "É obrigatório indicar o ano de publicação do livro")]
    [AnoNoPassadoValidator(ErrorMessage = "O livro não pode ter sido publicado no futuro")]
    public ushort Ano { get; set; }

    [Display (Name = "ISBN-13")]
    [Required(ErrorMessage = "É obrigatório indicar o ISBN")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "O ISBN tem 13 dígitos")]
    public string ISBN { get; set; }

    [Display (Name = "Editora")]
    public int EditoraID { get; set; }

    public SelectList? EditoraInputSelect { get; set; }

}

public class AnoNoPassadoValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var ano = Convert.ToInt32(value);
        if (ano > DateOnly.FromDateTime(DateTime.Now).Year || ano < 0)
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}
