using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.Venda
{
    public class VendaMapeamento : BaseMapeamento<Venda>
    {
        public override void Configure(EntityTypeBuilder<Venda> builder)
        {
            base.Configure(builder);
            builder.ToTable("VENDA");

            builder.Property(v => v.CodigoVenda)
                .HasColumnName("CODIGO_VENDA")
                .IsRequired();

            builder.HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v=>v.IdCliente)
                .IsRequired();
            builder.Property(v => v.IdCliente)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_CLIENTE")
                .IsRequired();
            builder.Property(v => v.DescontoTotal)
                .HasColumnName("DESCONTO_TOTAL");
            builder.Property(v=>v.ValorTotalComDesconto)
                .HasColumnName("VALOR_TOTAL_COM_DESCONTO")
                .IsRequired();
            builder.Property(v => v.ValorTotalSemDesconto)
                .HasColumnName("VALOR_TOTAL_SEM_DESCONTO")
                .IsRequired();
            builder.HasOne(v=>v.Vendedor)
                .WithMany()
                .HasForeignKey(v=>v.VendedorId)
                .IsRequired();
            builder.Property(v=>v.VendedorId)
                .HasColumnName("ID_VENDEDOR")
                .IsRequired();
        }
    }
}
