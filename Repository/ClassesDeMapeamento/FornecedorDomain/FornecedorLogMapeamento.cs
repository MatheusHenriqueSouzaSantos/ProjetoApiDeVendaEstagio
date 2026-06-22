using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.FornecedorDomain
{
    public class FornecedorLogMapeamento : BaseLogMapeamento<FornecedorLog>
    {
        public override void Configure(EntityTypeBuilder<FornecedorLog> builder)
        {
            base.Configure(builder);

            builder.ToTable("LOG_FORNECEDOR");

            builder.HasOne(f=>f.Fornecedor)
                .WithMany()
                .HasForeignKey(f=>f.IdFornecedor)
                .IsRequired();
            builder.Property(f=>f.IdFornecedor)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_FORNECEDOR")
                .IsRequired();

        }
    }
}
