using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.Venda.Transacao
{
    public class ParcelaMapeamento : BaseMapeamento<Parcela>
    {
        public override void Configure(EntityTypeBuilder<Parcela> builder)
        {
            base.Configure(builder);
            builder.ToTable("PARCELA");

            builder.HasOne(p => p.Transacao)
                .WithMany()
                .HasForeignKey(p=>p.IdTransacao)
                .IsRequired();
            builder.Property(p=>p.IdTransacao)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_TRANSACAO")
                .IsRequired();
            builder.Property(p => p.NumeroDaParecelaDaVenda)
                .HasColumnName("NUMERO_DA_PARCELA_DA_VENDA")
                .IsRequired();
            builder.Property(p=>p.ValorParcela)
                .HasColumnName("VALOR_PARCELA")
                .IsRequired();
            builder.Property(p=>p.Pago)
                .HasColumnName("PAGO")
                .IsRequired();
        }
    }
}
