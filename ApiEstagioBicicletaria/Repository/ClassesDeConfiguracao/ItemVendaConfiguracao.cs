using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ItemVendaConfiguracao : IEntityTypeConfiguration<ItemVenda>
    {
        public void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            builder.ToTable("item_venda");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.HasOne(i => i.Venda)
                .WithMany()
                .HasForeignKey("ID_VENDA")
                .IsRequired();
            builder.HasOne(i=>i.Produto)
                .WithMany()
                .HasForeignKey("ID_PRODUTO")
                .IsRequired();
            builder.Property(i=>i.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(i=>i.Quantidade)
                .HasColumnName("QUANTIDADE")
                .IsRequired();
            builder.Property(i=>i.DescontoUnitario)
                .HasColumnName("DESCONTO_UNITARIO")
                .IsRequired();
            builder.Property(i=>i.PrecoUnitarioNaVenda)
                .HasColumnName("PRECO_UNITARIO_NA_VENDA")
                .IsRequired();
            builder.Property(i=>i.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
