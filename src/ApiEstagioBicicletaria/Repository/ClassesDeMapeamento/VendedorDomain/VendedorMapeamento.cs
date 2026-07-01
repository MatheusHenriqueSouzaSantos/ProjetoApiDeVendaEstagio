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
            builder.ToTable("vendedor");

            builder.Property(v => v.Telefone)
                .IsRequired()
                .HasColumnName("telefone")
                .HasMaxLength(20);

            builder.Property(v => v.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(200);

            builder.Property(v => v.NomeCompleto)
                .IsRequired()
                .HasColumnName("nome_completo")
                .HasMaxLength(100);

            builder.Property(v => v.Cpf)
                .IsRequired()
                .HasColumnName("cpf")
                .HasMaxLength(11);

        }
    }
}
