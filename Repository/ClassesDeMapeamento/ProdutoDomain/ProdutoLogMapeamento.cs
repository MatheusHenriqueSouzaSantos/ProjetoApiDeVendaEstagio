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
            builder.ToTable("LOG_PRODUTO");

            builder.HasOne(p => p.Produto)
                .WithMany()
                .HasForeignKey(p => p.IdProduto)
                .IsRequired();

            builder.Property(p => p.IdProduto)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_PRODUTO")
                .IsRequired();
        }
    }
}
