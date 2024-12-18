using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_EF.Exemplo1.Models
{
    public class AutorEditViewModel
    {
        public int AutorID { get; set; }

        [Display(Name = "Nome do Autor")]
        [Required(ErrorMessage = "O nome do autor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do autor deve ter até 100 caracteres.")]
        public string AutorNome { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateOnly? AutorDataNascimento { get; set; } // Alterado para DateOnly?

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "O e-mail não é válido.")]
        public string AutorEmail { get; set; }
    }
}