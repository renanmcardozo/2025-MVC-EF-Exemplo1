using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MVC_EF.Exemplo1.Models
{
    // Definição do modelo de Autor
    public class Autor
    {
        public int AutorID { get; set; }  // Identificador único do autor
        public string AutorNome { get; set; }  // Nome do autor
        public DateOnly? AutorDataNascimento { get; set; }  // Data de nascimento do autor (opcional)
        public string? AutorEmail { get; set; }  // E-mail do autor (opcional)

        // Relação com a entidade Livro (muitos para muitos)
        public ICollection<AutorLivro>? LivrosDoAutor { get; set; }  
    }

    // Configuração de como a entidade Autor será mapeada para o banco de dados
    public class AutorConfiguration : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.HasKey(p => p.AutorID);  // Define a chave primária

            builder.HasIndex(p => p.AutorNome);  // Cria um índice para o nome do autor

            builder.Property(p => p.AutorNome)  // Define as propriedades
                .HasMaxLength(80)
                .IsRequired();  // Nome do autor é obrigatório e tem um limite de 80 caracteres

            builder.Property(p => p.AutorEmail)
                .HasMaxLength(80);  // E-mail do autor (máximo de 80 caracteres)

            builder.Property(p => p.AutorDataNascimento)
                .HasDefaultValue(DateOnly.FromDateTime(DateTime.Parse("1970/01/01")));  // Define um valor padrão para a data de nascimento
        }
    }
}