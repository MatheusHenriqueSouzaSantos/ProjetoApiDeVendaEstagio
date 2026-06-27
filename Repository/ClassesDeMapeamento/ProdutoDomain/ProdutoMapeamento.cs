using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ProdutoDomain
{
    public class ProdutoMapeamento : BaseMapeamento<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            base.Configure(builder);

            builder.ToTable("produto");
        
            builder.Property(p => p.CodigoDeBarra)
                .HasColumnName("codigo_de_barra")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(p => p.NomeProduto)
                .HasColumnName("nome_produto")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Descricao)
                .HasMaxLength(150)
                .HasColumnName("descricao");
            builder.Property(p => p.Preco)
                .HasColumnName("preco_unitario")
                .IsRequired();
        }
    }
}
