using ApiEstagioBicicletaria.Entities.ServicoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ServicoDomain
{
    public class ServicoLogMapeamento : BaseLogMapeamento<ServicoLog>
    {
        public override void Configure(EntityTypeBuilder<ServicoLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("log_servico");

            builder.HasOne(s => s.Servico)
                .WithMany()
                .HasForeignKey(s => s.IdServico)
                .IsRequired();
            builder.Property(s => s.IdServico)
                .HasColumnName("id_servico")
                .IsRequired();
        }
    }
}
