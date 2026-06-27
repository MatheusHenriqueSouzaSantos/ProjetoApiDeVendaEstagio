using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoqueDomain.ItemEntradaDomain;

public class ItemEntradaEstoqueMapeamento : BaseMapeamento<ItemEntradaEstoque>
{
    public override void Configure(EntityTypeBuilder<ItemEntradaEstoque> builder)
    {
        base.Configure(builder);

        builder.ToTable("item_entrada_estoque");

        builder.HasOne(i=>i.EntradaEstoque)
            .WithMany(e=>e.Itens)
            .HasForeignKey(i=>i.IdEntradaEstoque)
            .IsRequired();

        builder.Property(i=>i.IdEntradaEstoque)
            .HasColumnName("id_entrada_estoque")
            .IsRequired();

        builder.HasOne(i=>i.Produto)
            .WithMany()
            .HasForeignKey(i=>i.IdProduto)
            .IsRequired();
        builder.Property(i=>i.IdProduto)
            .HasColumnName("id_produto")
            .IsRequired();
        builder.Property(i=>i.Quantidade)
            .HasColumnName("quantidade")
            .IsRequired();
            
    }
}
