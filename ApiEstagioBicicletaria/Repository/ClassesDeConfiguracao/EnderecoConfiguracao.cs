using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class EnderecoConfiguracao : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder) 
        {
            builder.ToTable("endereco");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnType("Binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.Property(e => e.Logradouro)
                .HasMaxLength(80)
                .HasColumnName("LOGRADOURO")
                .IsRequired();
            builder.Property(e => e.Numero)
                .HasMaxLength(15)
                .HasColumnName("NUMERO")
                .IsRequired();
            builder.Property(e=>e.Cidade)
                .HasMaxLength(35)
                .HasColumnName("CIDADE")
                .IsRequired();
            builder.Property(e=>e.SiglaUf)
                .HasMaxLength(2)
                .HasColumnName("SIGLA_UF")
                .IsRequired();
        }
    }
}
