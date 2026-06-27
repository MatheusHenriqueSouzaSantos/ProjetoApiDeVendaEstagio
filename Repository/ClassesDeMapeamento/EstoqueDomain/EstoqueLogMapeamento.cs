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

            builder.ToTable("log_estoque");

            builder.HasOne(e => e.Estoque)
                .WithMany()
                .HasForeignKey(e => e.IdEstoque)
                .IsRequired();
            builder.Property(e => e.IdEstoque)
                .HasColumnName("id_estoque")
                .IsRequired();
            builder.HasOne(e => e.Produto)
               .WithMany()
               .HasForeignKey(e => e.IdProduto)
               .IsRequired();
            builder.Property(e => e.IdProduto)
                .HasColumnName("id_produto")
                .IsRequired();
            builder.Property(e => e.AcaoQueAlterouEstoque)
                .HasColumnName("acao_que_alterou_estoque")
                .IsRequired();

        }
    }
}
