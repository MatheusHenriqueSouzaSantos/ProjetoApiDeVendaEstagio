using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EstoqueDomain
{
    public class EstoqueLogMapeamento : BaseLogMapeamento<EstoqueLog>
    {
        public override void Configure(EntityTypeBuilder<EstoqueLog> builder)
        {
            base.Configure(builder);

            builder.ToTable("LOG_ESTOQUE");

            builder.HasOne(e => e.Estoque)
                .WithMany()
                .HasForeignKey(e => e.IdEstoque)
                .IsRequired();
            builder.Property(e => e.IdEstoque)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_ESTOQUE")
                .IsRequired();
            builder.HasOne(e => e.Produto)
               .WithMany()
               .HasForeignKey(e => e.IdProduto)
               .IsRequired();
            builder.Property(e => e.IdProduto)
                .HasColumnName("ID_PRODUTO")
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.Property(e => e.AcaoQueAlterouEstoque)
                .HasColumnName("ACAO_QUE_ALTEROU_ESTOQUE")
                .IsRequired();

        }
    }
}
