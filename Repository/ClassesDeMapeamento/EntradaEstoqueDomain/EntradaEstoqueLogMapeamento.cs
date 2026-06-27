using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoqueDomain
{
    public class EntradaEstoqueLogMapeamento : BaseLogMapeamento<EntradaEstoqueLog>
    {
        public override void Configure(EntityTypeBuilder<EntradaEstoqueLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("log_entrada_estoque");
            builder.HasOne(e=>e.EntradaEstoque)
                .WithMany()
                .HasForeignKey(e=>e.IdEntradaEstoque);
            builder.Property(e=>e.IdEntradaEstoque)
                .HasColumnName("id_entrada_estoque")
                .IsRequired();
        }
    }
}
