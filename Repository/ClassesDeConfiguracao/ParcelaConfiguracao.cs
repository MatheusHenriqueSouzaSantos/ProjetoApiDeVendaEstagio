using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ParcelaConfiguracao : IEntityTypeConfiguration<Parcela>
    {
        public void Configure(EntityTypeBuilder<Parcela> builder)
        {
            builder.ToTable("parcela");

            builder.HasKey(p=>p.Id);

            builder.Property(p => p.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.HasOne(p => p.Transacao)
                .WithMany()
                .HasForeignKey(p=>p.IdTransacao)
                .IsRequired();
            builder.Property(p=>p.IdTransacao)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_TRANSACAO")
                .IsRequired();
            builder.Property(p => p.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
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
            builder.Property(p=>p.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
