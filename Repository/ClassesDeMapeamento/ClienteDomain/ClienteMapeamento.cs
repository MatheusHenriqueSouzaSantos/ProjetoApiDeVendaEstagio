using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain
{
    public class ClienteMapeamento : BaseMapeamento<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            base.Configure(builder);
            builder.ToTable("CLIENTE");

            builder.HasOne(c => c.Endereco)
                .WithMany()
                .HasForeignKey(c=>c.IdEndereco)
                .IsRequired();
            builder.Property(c => c.IdEndereco)
                .HasColumnName("ID_ENDERECO")
                .IsRequired();
            builder.Property(c => c.Telefone)
                .HasMaxLength(2)
                .HasColumnName("TELEFONE")
                .IsRequired();
            builder.Property(c=>c.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL")
                .IsRequired();
            builder.Property(c=>c.TipoCliente)
                .HasConversion<string>()
                .HasMaxLength(25)
                .HasColumnName("TIPO_CLIENTE")
                .IsRequired();

        }
    }
}
