using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ClienteJuridicoConfiguracao : IEntityTypeConfiguration<ClienteJuridico>
    {
        public void Configure(EntityTypeBuilder<ClienteJuridico> builder)
        {
            builder.ToTable("cliente_juridico");
            //builder.HasKey(c => c.Id);

            //banco já faz isso:

            builder.Property(c => c.Id)
                .HasColumnType("Binary(16)")
                .IsRequired();


            builder.Property(c => c.RazaoSocial)
                .HasMaxLength(70)
                .HasColumnName("RAZAO_SOCIAL")
                .IsRequired();
            builder.Property(c => c.NomeFantasia)
                .HasColumnName("NOME_FANTASIA")
                .HasMaxLength(30);
            builder.Property(c => c.InscricaoEstadual)
                .HasColumnName("INSCRICAO_ESTADUAL")
                .HasMaxLength(10);
            builder.Property(c=>c.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("CNPJ")
                .IsRequired();
        }
    }
}
