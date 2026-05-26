using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.Venda
{
    public class ItemVendaMapeamento : BaseMapeamento<ItemVenda>
    {
        public override void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            base.Configure(builder);
            builder.ToTable("ITEM_VENDA");

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
            builder.Property(i=>i.Quantidade)
                .HasColumnName("QUANTIDADE")
                .IsRequired();
            builder.Property(i=>i.DescontoUnitario)
                .HasColumnName("DESCONTO_UNITARIO")
                .IsRequired();
            builder.Property(i=>i.PrecoUnitarioDoProdutoNaVendaSemDesconto)
                .HasColumnName("PRECO_UNITARIO_DO_PRODUTO_NA_VENDA_SEM_DESCONTO")
                .IsRequired();
        }
    }
}
