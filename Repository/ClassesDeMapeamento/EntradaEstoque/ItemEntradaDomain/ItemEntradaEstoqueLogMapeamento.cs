using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoque.ItemEntradaDomain
{
    public class ItemEntradaEstoqueLogMapeamento : BaseLogMapeamento<ItemEntradaEstoqueLog>
    {
        public override void Configure(EntityTypeBuilder<ItemEntradaEstoqueLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_ITEM_ENTRADA_ESTOQUE");
            builder.HasOne(i => i.ItemEntradaEstoque)
                .WithMany()
                .HasForeignKey(i=>i.IdItemEntradaEstoque);
            builder.Property(i => i.IdItemEntradaEstoque)
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.HasOne(i=>i.EntradaEstoque)
                .WithMany()
                .HasForeignKey(i=>i.IdEntradaEstoque)
                .IsRequired();
            builder.Property(i=>i.IdEntradaEstoque)
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    }
}
