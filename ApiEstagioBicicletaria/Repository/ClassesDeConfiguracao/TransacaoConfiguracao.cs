using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class TransacaoConfiguracao : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("transacao");

            builder.HasKey(t=>t.Id);

            builder.Property(t => t.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.HasOne(t => t.Venda)
                .WithOne()
                .HasForeignKey("ID_VENDA")
                .IsRequired();
            builder.Property(t => t.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
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
            builder.Property(t=>t.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
