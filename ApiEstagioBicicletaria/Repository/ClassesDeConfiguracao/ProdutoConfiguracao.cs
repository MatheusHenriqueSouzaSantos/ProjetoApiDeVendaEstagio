using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ProdutoConfiguracao : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produto");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.Property(p => p.CodigoDeBarra)
                .HasColumnName("CODIGO_DE_BARRA")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(p => p.NomeProduto)
                .HasColumnName("NOME_PRODUTO")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Descricao)
                .HasMaxLength(150)
                .HasColumnName("DESCRICAO");
            builder.Property(p => p.QuantidadeEmEstoque)
                .HasColumnName("QUANTIDADE_EM_ESTOQUE")
                .IsRequired();
            builder.Property(p => p.PrecoUnitario)
                .HasColumnName("PRECO_UNITARIO")
                .IsRequired();
        }
    }
}
