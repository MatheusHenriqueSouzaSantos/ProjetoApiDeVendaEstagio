using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ServicoConfiguracao : IEntityTypeConfiguration<Servico>
    {
        public void Configure(EntityTypeBuilder<Servico> builder)
        {
            builder.ToTable("servico");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.Property(p => p.CodigoServico)
                .HasColumnName("CODIGO_SERVICO")
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(p => p.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(p => p.NomeServico)
                .HasColumnName("NOME_SERVICO")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Descricao)
                .HasMaxLength(150)
                .HasColumnName("DESCRICAO");
            builder.Property(p => p.PrecoServico)
                .HasColumnName("PRECO_SERVICO")
                .IsRequired();
            builder.Property(p => p.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
