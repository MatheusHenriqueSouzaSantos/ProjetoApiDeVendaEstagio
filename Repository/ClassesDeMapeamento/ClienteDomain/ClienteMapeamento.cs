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
            builder.ToTable("cliente");

            builder.HasOne(c => c.Endereco)
                .WithMany()
                .HasForeignKey(c=>c.IdEndereco)
                .IsRequired();
            builder.Property(c => c.IdEndereco)
                .HasColumnName("id_endereco")
                .IsRequired();
            builder.Property(c => c.Telefone)
                .HasMaxLength(2)
                .HasColumnName("telefone")
                .IsRequired();
            builder.Property(c=>c.Email)
                .HasMaxLength(100)
                .HasColumnName("email")
                .IsRequired();
            builder.Property(c=>c.TipoCliente)
                .HasConversion<string>()
                .HasMaxLength(25)
                .HasColumnName("tipo_cliente")
                .IsRequired();

        }
    }
}
