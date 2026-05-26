using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoque;

public class ItemEntradaEstoqueMapeamento : BaseMapeamento<ItemEntradaEstoque>
{
    public override void Configure(EntityTypeBuilder<ItemEntradaEstoque> builder)
    {
        base.Configure(builder);

        builder.ToTable("ITEM_ENTRADA_ESTOQUE");

        builder.HasOne(i=>i.EntradaEstoque)
            .WithMany(e=>e.Itens)
            .HasForeignKey(i=>i.IdEntradaEstoque)
            .IsRequired();

        builder.Property(i=>i.IdEntradaEstoque)
            .HasColumnType("binary(16)")
            .HasColumnName("ID_ENTRADA_ESTOQUE")
            .IsRequired();

        builder.HasOne(i=>i.Estoque)
            .WithMany()
            .HasForeignKey(i=>i.IdEstoque)
            .IsRequired();
        builder.Property(i=>i.IdEstoque)
            .HasColumnType("binary(16)")
            .HasColumnName("ID_ESTOQUE")
            .IsRequired();
        builder.Property(i=>i.Quantidade)
            .HasColumnName("QUANTIDADE")
            .IsRequired();
            
    }
}
