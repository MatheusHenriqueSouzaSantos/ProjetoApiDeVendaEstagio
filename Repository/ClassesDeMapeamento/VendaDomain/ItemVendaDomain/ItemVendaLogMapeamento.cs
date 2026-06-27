using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.ItemVendaDomain
{
    public class ItemVendaLogMapeamento : BaseLogMapeamento<ItemVendaLog>
    {
        public override void Configure(EntityTypeBuilder<ItemVendaLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("log_item_venda");
            builder.HasOne(i => i.ItemVenda)
                .WithMany()
                .HasForeignKey(i => i.IdItemVenda);
            builder.Property(i => i.IdItemVenda)
                .HasColumnName("id_item_venda")
                .IsRequired();
            builder.HasOne(i => i.Venda)
                .WithMany()
                .HasForeignKey(i => i.IdVenda)
                .IsRequired();
            builder.Property(i => i.IdVenda)
                .HasColumnName("id_venda")
                .IsRequired();
        }

    }
}
