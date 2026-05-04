using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria;

public class EntradaEstoqueMapeamento : BaseMapeamento<EntradaEstoque>
{
    public override void Configure(EntityTypeBuilder<EntradaEstoque> builder)
    {
        base.Configure(builder);

        builder.ToTable("ENTRADA_ESTOQUE");

        builder.HasOne(e=>e.Fornecedor)
            .WithMany()
            .HasForeignKey(e=>e.IdFornecedor)
            .IsRequired();
        builder.Property(e=>e.IdFornecedor)
            .HasColumnType("binary(16)")
            .HasColumnName("ID_FORNECEDOR")
            .IsRequired();
        builder.Property(e=>e.CodigoEntrada)
            .HasColumnName("CODIGO_ENTRADA")
            .IsRequired();
        builder.Property(e => e.Status)
            .HasColumnName("STATUS")
            .IsRequired();
        
    }
}
