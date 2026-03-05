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
                .HasForeignKey(i=>i.IdVenda)
                .IsRequired();
            builder.Property(i => i.IdVenda)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_VENDA");
            builder.HasOne(i=>i.Produto)
                .WithMany()
                .HasForeignKey(i=>i.IdProduto)
                .IsRequired();
            builder.Property(i=>i.IdProduto)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_PRODUTO")
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
            builder.Property(i=>i.PrecoUnitarioDoProdutoNaVendaSemDesconto)
                .HasColumnName("PRECO_UNITARIO_DO_PRODUTO_NA_VENDA_SEM_DESCONTO")
                .IsRequired();
            builder.Property(i=>i.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
