using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.ItemVendaDomain
{
    public class ItemVendaMapeamento : BaseMapeamento<ItemVenda>
    {
        public override void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            base.Configure(builder);
            builder.ToTable("item_venda");

            builder.HasOne(i => i.Venda)
                .WithMany()
                .HasForeignKey(i=>i.IdVenda)
                .IsRequired();
            builder.Property(i => i.IdVenda)
                .HasColumnName("id_venda");
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
            builder.Property(i=>i.DescontoUnitario)
                .HasColumnName("desconto_unitario")
                .IsRequired();
            builder.Property(i=>i.PrecoUnitarioDoProdutoNaVendaSemDesconto)
                .HasColumnName("preco_unitario_do_produto_na_venda_sem_desconto")
                .IsRequired();
        }
    }
}
