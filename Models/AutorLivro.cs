using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MVC_EF.Exemplo1.Models;

public class AutorLivro
{
    public int LivroID { get; set; }
    public int AutorID { get; set; }
    public ushort OrdemAutoria { get; set; }

    public Livro Livro { get; set; }
    public Autor Autor { get; set; }
}

public class AutorLivroConfiguration : IEntityTypeConfiguration<AutorLivro>
{
    public void Configure(EntityTypeBuilder<AutorLivro> builder)
    {
        builder.HasKey(p => new { p.AutorID, p.LivroID });

        builder.Property(p => p.AutorID).IsRequired();
        builder.Property(p => p.LivroID).IsRequired();
        builder.Property(p => p.OrdemAutoria).HasDefaultValue(0).IsRequired();

        builder.HasOne<Livro>(p => p.Livro)
            .WithMany(p => p.AutoresDoLivro).HasForeignKey(p => p.LivroID);
        builder.HasOne<Autor>(p => p.Autor)
            .WithMany(p => p.LivrosDoAutor).HasForeignKey(p => p.AutorID);

    }
}