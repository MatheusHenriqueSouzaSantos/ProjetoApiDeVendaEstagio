using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.Produto
{
    public class ProdutoMapeamento : BaseMapeamento<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            base.Configure(builder);

            builder.ToTable("PRODUTO");
        
            builder.Property(p => p.CodigoDeBarra)
                .HasColumnName("CODIGO_DE_BARRA")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(p => p.NomeProduto)
                .HasColumnName("NOME_PRODUTO")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Descricao)
                .HasMaxLength(150)
                .HasColumnName("DESCRICAO");
            builder.Property(p => p.Preco)
                .HasColumnName("PRECO_UNITARIO")
                .IsRequired();
        }
    }
}
