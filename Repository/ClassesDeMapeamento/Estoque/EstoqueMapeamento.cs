using ApiEstagioBicicletaria.Entities.EstoqueDomain.Estoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.Estoque
{
    public class EstoqueMapeamento : BaseMapeamento<Estoque>
    {
        public override void Configure(EntityTypeBuilder<Estoque> builder)
        {
            base.Configure(builder);
            builder.ToTable("ESTOQUE");

            builder.HasOne(e => e.Produto)
                .WithOne()
                .HasForeignKey<Estoque>(e=>e.ProdutoId)
                .IsRequired();

            builder.Property(e => e.ProdutoId)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_PRODUTO");

            builder.Property(e=>e.QuantidadeEmEstoque)
                .HasColumnName("QUANTIDADE_EM_ESTOQUE")
                .IsRequired();
        }
    }
}
