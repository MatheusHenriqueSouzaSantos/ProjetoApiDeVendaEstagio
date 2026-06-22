using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.FinancialTransactionDomain
{
    public class TransacaoLogMapeamento : BaseLogMapeamento<TransacaoLog>
    {
        public override void Configure(EntityTypeBuilder<TransacaoLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_TRANSACAO");

            builder.HasOne(t => t.Transacao)
                .WithMany()
                .HasForeignKey(t => t.IdTransacao)
                .IsRequired();
            builder.Property(t => t.IdTransacao)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_TRANSACAO")
                .IsRequired();
            builder.HasOne(t => t.Venda)
                .WithMany()
                .HasForeignKey(t => t.IdVenda);
            builder.Property(t => t.IdVenda)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_VENDA")
                .IsRequired();
        }
    }
}
