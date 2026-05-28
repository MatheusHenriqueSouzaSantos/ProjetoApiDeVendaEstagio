using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.ServicoVendaDomain
{
    public class ServicoVendaLogMapeamento : BaseLogMapeamento<ServicoVendaLog>
    {
        public override void Configure(EntityTypeBuilder<ServicoVendaLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_SERVICO_VENDA");
            builder.HasOne(s => s.ServicoVenda)
                .WithMany()
                .HasForeignKey(i => i.IdServicoVenda);
            builder.Property(i => i.IdServicoVenda)
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.HasOne(i => i.Venda)
                .WithMany()
                .HasForeignKey(i => i.IdVenda)
                .IsRequired();
            builder.Property(i => i.IdVenda)
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    }
}
