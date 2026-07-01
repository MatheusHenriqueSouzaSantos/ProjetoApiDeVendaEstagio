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
            builder.ToTable("log_endereco");
            builder.HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey(e => e.IdEndereco);
            builder.Property(e => e.IdEndereco)
                .HasColumnName("id_endereco")
                .IsRequired();
            builder.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.IdCliente)
                .IsRequired();
            builder.Property(e=>e.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();

        }
    }
}
