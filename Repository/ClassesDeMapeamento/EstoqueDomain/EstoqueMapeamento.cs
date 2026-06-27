using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EstoqueDomain
{
    public class EstoqueMapeamento : BaseMapeamento<Estoque>
    {
        public override void Configure(EntityTypeBuilder<Estoque> builder)
        {
            base.Configure(builder);
            builder.ToTable("estoque");

            builder.HasOne(e => e.Produto)
                .WithOne()
                .HasForeignKey<Estoque>(e=>e.ProdutoId)
                .IsRequired();

            builder.Property(e => e.ProdutoId)
                .HasColumnName("id_produto");

            builder.Property(e=>e.QuantidadeEmEstoque)
                .HasColumnName("quantidade_em_estoque")
                .IsRequired();
        }
    }
}
