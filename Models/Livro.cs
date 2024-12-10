using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MVC_EF.Exemplo1.Models;

public class Livro
{
    public int LivroID { get; set; }
    public string LivroTitulo { get; set; }
    public ushort LivroPaginas { get; set; }
    public string LivroISBN { get; set; }
    public ushort LivroAnoPublicacao { get; set; }
    public int EditoraID { get; set; }

    public Editora EditoraDoLivro { get; set; }
    public ICollection<OperacaoCompraVenda>? OperacoesDoLivro { get; set; }
    public ICollection<AutorLivro>? AutoresDoLivro { get; set; }
}

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.HasKey(p => p.LivroID);
        builder.Property(p => p.LivroTitulo).HasMaxLength(120).IsRequired();
        builder.Property(p => p.LivroPaginas).HasDefaultValue(0).IsRequired();
        builder.Property(p => p.LivroISBN).HasMaxLength(13).IsRequired();
        
        builder.HasOne<Editora>(p => p.EditoraDoLivro).WithMany(p => p.LivrosDaEditora);
    }
}
