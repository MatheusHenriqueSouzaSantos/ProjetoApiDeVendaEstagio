using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain
{
    public class ClienteJuridicoMapeamento : IEntityTypeConfiguration<ClienteJuridico>
    {
        public void Configure(EntityTypeBuilder<ClienteJuridico> builder)
        {
            builder.ToTable("cliente_juridico");
            //builder.HasKey(c => c.Id);

            //banco já faz isso:

            builder.Property(c => c.Id)
                .IsRequired();


            builder.Property(c => c.RazaoSocial)
                .HasMaxLength(70)
                .HasColumnName("razao_social")
                .IsRequired();
            builder.Property(c => c.NomeFantasia)
                .HasColumnName("nome_fantasia")
                .HasMaxLength(30);
            builder.Property(c => c.InscricaoEstadual)
                .HasColumnName("inscricao_estadual")
                .HasMaxLength(10);
            builder.Property(c=>c.Cnpj)
                .HasMaxLength(14)
                .HasColumnName("cnpj")
                .IsRequired();
        }
    }
}
