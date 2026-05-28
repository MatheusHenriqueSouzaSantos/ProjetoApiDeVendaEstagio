using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.FinancialTransactionDomain.ParcelaDomain
{
    public class ParcelaLogMapeamento : BaseLogMapeamento<ParcelaLog>
    {
        public override void Configure(EntityTypeBuilder<ParcelaLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_PARCELA");
            builder.HasOne(p => p.Parcela)
                .WithMany()
                .HasForeignKey(p => p.IdParcela)
                .IsRequired();
            builder.Property(p => p.IdParcela)
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.HasOne(p => p.Transacao)
                .WithMany()
                .HasForeignKey(p => p.IdTransacao);
            builder.Property(p=>p.IdTransacao)
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    }
}
