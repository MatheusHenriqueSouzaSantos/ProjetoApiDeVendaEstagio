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
            builder.ToTable("LOG_CLIENTE");
            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente)
                .IsRequired();
            builder.Property(c => c.IdCliente)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_CLIENTE")
                .IsRequired();
            builder.Property(c => c.TipoCliente)
                .HasColumnName("TIPO_CLIENTE")
                .IsRequired();
           
        }
    }
}
