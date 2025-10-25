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
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnType("Binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.Property(c => c.Logradouro)
                .HasMaxLength(80)
                .HasColumnName("LOGRADOURO")
                .IsRequired();
            builder.Property(c => c.Numero)
                .HasMaxLength(15)
                .HasColumnName("NUMERO")
                .IsRequired();
            builder.Property(c=>c.Cidade)
                .HasMaxLength(35)
                .HasColumnName("CIDADE")
                .IsRequired();
            builder.Property(c=>c.SiglaUf)
                .HasMaxLength(2)
                .HasColumnName("SIGLA_UF")
                .IsRequired();
        }
    }
}
