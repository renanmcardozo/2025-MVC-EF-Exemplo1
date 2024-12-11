using System.ComponentModel.DataAnnotations;

namespace MVC_EF.Exemplo1.Models;

public class EditoraEditViewModel
{
    public int EditoraID { get; set; }

    [Display (Name = "Nome da editora")]
    [Required(ErrorMessage = "O nome da editora é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter até 80 caracteres")]
    public string EditoraNome { get; set; }

    [Display (Name = "Logradouro")]
    public string? EditoraLogradouro { get; set; }
    [Display (Name = "Número")]
    public ushort? EditoraNumero { get; set; }
    [Display (Name = "Complemento")]
    public string? EditoraComplemento { get; set; }
    [Display (Name = "Cidade")]
    public string? EditoraCidade { get; set; }
    [Display (Name = "UF")]
    public string? EditoraUF { get; set; }
    [Display (Name = "País")]
    public string? EditoraPais { get; set; }
    [Display (Name = "CEP")]
    public string? EditoraCEP { get; set; }
    [Display (Name = "Telefone")]
    public string? EditoraTelefone { get; set; }
}
