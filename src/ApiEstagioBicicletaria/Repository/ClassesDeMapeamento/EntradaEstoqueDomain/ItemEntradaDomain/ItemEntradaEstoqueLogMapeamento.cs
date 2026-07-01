using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoqueDomain
{
    public class ItemEntradaEstoqueLogMapeamento : BaseLogMapeamento<ItemEntradaEstoqueLog>
    {
        public override void Configure(EntityTypeBuilder<ItemEntradaEstoqueLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("log_item_entrada_estoque");
            builder.HasOne(i => i.ItemEntradaEstoque)
                .WithMany()
                .HasForeignKey(i=>i.IdItemEntradaEstoque);
            builder.Property(i => i.IdItemEntradaEstoque)
                .HasColumnName("id_item_entrada_estoque")
                .IsRequired();
            builder.HasOne(i=>i.EntradaEstoque)
                .WithMany()
                .HasForeignKey(i=>i.IdEntradaEstoque)
                .IsRequired();
            builder.Property(i=>i.IdEntradaEstoque)
                .HasColumnName("id_entrada_estoque")
                .IsRequired();
        }
    }
}
