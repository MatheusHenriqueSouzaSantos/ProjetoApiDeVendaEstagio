using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain
{
    public class ClienteLogMapeamento : BaseLogMapeamento<ClienteLog>
    {
        public override void Configure(EntityTypeBuilder<ClienteLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("log_cliente");
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente)
                .IsRequired();
            builder.Property(c => c.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();
            builder.Property(c => c.TipoCliente)
                .HasColumnName("tipo_cliente")
                .HasConversion<string>()
                .IsRequired();
           
        }
    }
}
