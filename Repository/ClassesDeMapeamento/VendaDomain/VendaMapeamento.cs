using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain
{
    public class VendaMapeamento : BaseMapeamento<Venda>
    {
        public override void Configure(EntityTypeBuilder<Venda> builder)
        {
            base.Configure(builder);
            builder.ToTable("venda");

            builder.Property(v => v.CodigoVenda)
                .HasColumnName("codigo_venda")
                .IsRequired();

            builder.HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v=>v.IdCliente)
                .IsRequired();
            builder.Property(v => v.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();
            builder.Property(v => v.DescontoTotal)
                .HasColumnName("desconto_total");
            builder.Property(v=>v.ValorTotalComDesconto)
                .HasColumnName("valor_total_com_desconto")
                .IsRequired();
            builder.Property(v => v.ValorTotalSemDesconto)
                .HasColumnName("valor_total_sem_desconto")
                .IsRequired();
            builder.HasOne(v=>v.Vendedor)
                .WithMany()
                .HasForeignKey(v=>v.IdVendedor)
                .IsRequired();
            builder.Property(v=>v.IdVendedor)
                .HasColumnName("id_vendedor")
                .IsRequired();
        }
    }
}
