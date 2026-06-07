using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendedorDomain
{
    public class VendedorLogMapeamento : BaseLogMapeamento<VendedorLog>
    {
        public override void Configure(EntityTypeBuilder<VendedorLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_VENDEDOR");
            builder.HasOne(v => v.Vendedor)
                .WithMany()
                .HasForeignKey(v => v.IdVendedor);
            builder.Property(v => v.IdVendedor)
                .HasColumnName("ID_VENDEDOR")
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    }
}
