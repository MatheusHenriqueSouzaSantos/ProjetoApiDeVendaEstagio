using ApiEstagioBicicletaria.Entities.VendedorDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendedorDomain
{
    public class VendedorMapeamento : BaseMapeamento<Vendedor>
    {
        public override void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            base.Configure(builder);
            builder.ToTable("VENDEDOR");

            builder.Property(v => v.Telefone)
                .IsRequired()
                .HasColumnName("TELEFONE")
                .HasMaxLength(20);

            builder.Property(v => v.Email)
                .IsRequired()
                .HasColumnName("EMAIL")
                .HasMaxLength(200);

            builder.Property(v => v.NomeCompleto)
                .IsRequired()
                .HasColumnName("NOME_COMPLETO")
                .HasMaxLength(100);

            builder.Property(v => v.Cpf)
                .IsRequired()
                .HasColumnName("CPF")
                .HasMaxLength(11);

        }
    }
}
