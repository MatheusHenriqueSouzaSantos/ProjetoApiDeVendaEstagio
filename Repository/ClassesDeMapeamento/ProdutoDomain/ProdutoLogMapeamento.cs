using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ProdutoDomain
{
    public class ProdutoLogMapeamento : BaseLogMapeamento<ProdutoLog>
    {
        public override void Configure(EntityTypeBuilder<ProdutoLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("log_produto");

            builder.HasOne(p => p.Produto)
                .WithMany()
                .HasForeignKey(p => p.IdProduto)
                .IsRequired();

            builder.Property(p => p.IdProduto)
                .HasColumnName("id_produto")
                .IsRequired();
        }
    }
}
