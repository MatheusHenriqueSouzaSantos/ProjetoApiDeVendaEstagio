using ApiEstagioBicicletaria.Entities.FornedorDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.FornecedorDomain
{
    public class FornecedorMapeamento : BaseMapeamento<Fornecedor>
    {
        public override void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            base.Configure(builder);

            builder.ToTable("fornecedor");

            builder.Property(f=>f.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(f=>f.Email)
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(f=>f.RazaoSocial)
                .HasColumnName("razao_social")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(f => f.NomeFantasia)
                .HasColumnName("nome_fantasia")
                .HasMaxLength(100);

            builder.Property(f=>f.Cnpj)
                .HasColumnName("cnpj")
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(f => f.InscricaoEstadual)
                .HasMaxLength(15)
                .HasColumnName("inscricao_estadual");

        }
    }
}
