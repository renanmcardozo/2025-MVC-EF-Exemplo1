using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MVC_EF.Exemplo1.Models;

public class OperacaoCompraVenda
{
    public int OperacaoID { get; set; }
    public int LivroID { get; set; }
    public DateOnly OperacaoData { get; set; }
    public short OperacaoQuantidade { get; set; }
    public Livro LivroDaOperacao { get; set; }
}

public class OperacaoCompraVendaConfiguration : IEntityTypeConfiguration<OperacaoCompraVenda>
{
    public void Configure(EntityTypeBuilder<OperacaoCompraVenda> builder)
    {
        builder.HasKey(p => p.OperacaoID);

        builder.HasIndex(p => p.OperacaoData);

        builder.HasOne<Livro>(p => p.LivroDaOperacao).WithMany(p => p.OperacoesDoLivro);

    }
}