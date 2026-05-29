using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain.EnderecoDomainMaper
{
    public class EnderecoLogMapeamento : BaseLogMapeamento<EnderecoLog>
    {
        public override void Configure(EntityTypeBuilder<EnderecoLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_ENDERECO");
            builder.HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey(e => e.IdEndereco);
            builder.Property(e => e.IdEndereco)
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.IdCliente)
                .IsRequired();
            builder.Property(e=>e.IdCliente)
                .HasColumnType("binary(16)")
                .IsRequired();

        }
    }
}
