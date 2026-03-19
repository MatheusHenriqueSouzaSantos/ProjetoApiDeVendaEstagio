
using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class EstoqueConfiguracao : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable("ESTOQUE");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnType("binary(16)")
                .IsRequired();

            builder.HasOne(e => e.Produto)
                .WithOne()
                .HasForeignKey<Estoque>(e=>e.ProdutoId)
                .IsRequired();

            builder.Property(e => e.ProdutoId)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_PRODUTO");

            builder.Property(e => e.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();


            builder.Property(e=>e.QuantidadeEmEstoque)
                .HasColumnName("QUANTIDADE_EM_ESTOQUE")
                .IsRequired();
        }
    }
}
