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
            builder.ToTable("LOG_VENDA");
            builder.HasOne(v => v.Venda)
                .WithMany()
                .HasForeignKey(v => v.IdVenda);
            builder.Property(v => v.IdVenda)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_VENDA")
                .IsRequired();
        }
    }
}
