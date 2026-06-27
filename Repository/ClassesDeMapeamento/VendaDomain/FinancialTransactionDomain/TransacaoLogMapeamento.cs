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
            builder.ToTable("log_transacao");

            builder.HasOne(t => t.Transacao)
                .WithMany()
                .HasForeignKey(t => t.IdTransacao)
                .IsRequired();
            builder.Property(t => t.IdTransacao)
                .HasColumnName("id_transacao")
                .IsRequired();
            builder.HasOne(t => t.Venda)
                .WithMany()
                .HasForeignKey(t => t.IdVenda);
            builder.Property(t => t.IdVenda)
                .HasColumnName("id_venda")
                .IsRequired();
        }
    }
}
