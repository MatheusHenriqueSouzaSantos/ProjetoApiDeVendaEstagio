using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain
{
    public class VendaLogMapeamento : BaseLogMapeamento<VendaLog>
    {
        public override void Configure(EntityTypeBuilder<VendaLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("log_venda");
            builder.HasOne(v => v.Venda)
                .WithMany()
                .HasForeignKey(v => v.IdVenda);
            builder.Property(v => v.IdVenda)
                .HasColumnName("id_venda")
                .IsRequired();
        }
    }
}
