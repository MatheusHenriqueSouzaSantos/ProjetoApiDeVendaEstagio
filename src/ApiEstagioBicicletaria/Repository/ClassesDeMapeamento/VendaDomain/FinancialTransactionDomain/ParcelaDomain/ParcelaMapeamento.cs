using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.FinancialTransactionDomain.ParcelaDomain
{
    public class ParcelaMapeamento : BaseMapeamento<Parcela>
    {
        public override void Configure(EntityTypeBuilder<Parcela> builder)
        {
            base.Configure(builder);
            builder.ToTable("parcela");

            builder.HasOne(p => p.Transacao)
                .WithMany()
                .HasForeignKey(p=>p.IdTransacao)
                .IsRequired();
            builder.Property(p=>p.IdTransacao)
                .HasColumnName("id_transacao")
                .IsRequired();
            builder.Property(p => p.NumeroDaParcelaDaVenda)
                .HasColumnName("numero_da_parcela_da_venda")
                .IsRequired();
            builder.Property(p=>p.ValorParcela)
                .HasColumnName("valor_parcela")
                .IsRequired();
            builder.Property(p=>p.Pago)
                .HasColumnName("pago")
                .IsRequired();
            builder.Property(p => p.DataVencimento)
             .HasColumnName("data_vencimento")
             .IsRequired();
        }
    }
}
