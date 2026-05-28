using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoque
{
    public class EntradaEstoqueLogMapeamento : BaseLogMapeamento<EntradaEstoqueLog>
    {
        public override void Configure(EntityTypeBuilder<EntradaEstoqueLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_ENTRADA_ESTOQUE");
            builder.HasOne(e=>e.EntradaEstoque)
                .WithMany()
                .HasForeignKey(e=>e.IdEntradaEstoque);
            builder.Property(e=>e.IdEntradaEstoque)
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    }
}
