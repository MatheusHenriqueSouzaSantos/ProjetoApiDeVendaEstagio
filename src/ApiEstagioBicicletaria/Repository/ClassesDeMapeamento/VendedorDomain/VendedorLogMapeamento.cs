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
            builder.ToTable("log_vendedor");
            builder.HasOne(v => v.Vendedor)
                .WithMany()
                .HasForeignKey(v => v.IdVendedor);
            builder.Property(v => v.IdVendedor)
                .HasColumnName("id_vendedor")
                .IsRequired();
        }
    }
}
