using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.Venda.Transacao
{
    public class TransacaoMapeamento : BaseMapeamento<Transacao>
    {
        public override void Configure(EntityTypeBuilder<Transacao> builder)
        {
            base.Configure(builder);

            builder.ToTable("TRANSACAO");

            builder.HasOne(t => t.Venda)
                .WithOne()
                .HasForeignKey<Transacao>(t=>t.IdVenda)
                .IsRequired();
            builder.Property(t => t.IdVenda)
                .HasColumnName("ID_VENDA")
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.Property(t => t.TipoPagamento)
                .HasColumnName("TIPO_PAGAMENTO")
                .HasConversion<string>()
                .IsRequired();
            builder.Property(t => t.MeioPagamento)
                .HasColumnName("MEIO_PAGAMENTO")
                .HasConversion<string>()
                .IsRequired();
            builder.Property(t => t.TransacaoEmCurso)
                .HasColumnName("TRANSACAO_EM_CURSO")
                .IsRequired();
            builder.Property(t => t.Pago)
                .HasColumnName("PAGO")
                .IsRequired();
        }
    }
}
