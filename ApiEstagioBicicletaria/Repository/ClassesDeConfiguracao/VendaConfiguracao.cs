using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class VendaConfiguracao : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.ToTable("venda");

            builder.HasKey(v=>v.Id);

            builder.Property(v => v.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v=>v.IdCliente)
                .IsRequired();
            builder.Property(v => v.IdCliente)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_CLIENTE")
                .IsRequired();
            builder.Property(v => v.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(v => v.Desconto)
                .HasColumnName("DESCONTO");
            builder.Property(v=>v.ValorTotal)
                .HasColumnName("VALOR_TOTAL")
                .IsRequired();
            builder.Property(v=>v.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
