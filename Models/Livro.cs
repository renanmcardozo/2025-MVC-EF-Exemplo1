using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MVC_EF.Exemplo1.Models;

public class Livro
{
    public int LivroID { get; set; }
    public string LivroTitulo { get; set; }
}

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.HasKey(p => p.LivroID);
        builder.Property(p => p.LivroTitulo).HasMaxLength(120).IsRequired();
    }
}