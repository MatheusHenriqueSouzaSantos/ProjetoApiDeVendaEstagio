using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class FornecedorMapeamento : BaseMapeamento<Fornecedor>
    {
        public override void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            base.Configure(builder);

            builder.ToTable("FORNECEDOR");

            builder.Property(f=>f.Telefone)
                .HasColumnName("TELEFONE")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(f=>f.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(f=>f.RazaoSocial)
                .HasColumnName("RAZAO_SOCIAL")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(f => f.NomeFantasia)
                .HasColumnName("NOME_FANTASIA")
                .HasMaxLength(100);

            builder.Property(f=>f.Cnpj)
                .HasColumnName("CNPJ")
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(f => f.InscricaoEstadual)
                .HasMaxLength(15)
                .HasColumnName("INSCRICAO_ESTADUAL");

        }
    }
}
