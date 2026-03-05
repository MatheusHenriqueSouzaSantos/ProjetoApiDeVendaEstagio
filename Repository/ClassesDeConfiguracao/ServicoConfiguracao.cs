using ApiEstagioBicicletaria.Entities.ServicoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ServicoConfiguracao : IEntityTypeConfiguration<Servico>
    {
        public void Configure(EntityTypeBuilder<Servico> builder)
        {
            builder.ToTable("servico");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.Property(s => s.CodigoDoServico)
                .HasColumnName("CODIGO_DO_SERVICO")
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(s => s.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(s => s.NomeServico)
                .HasColumnName("NOME_SERVICO")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(s => s.Descricao)
                .HasMaxLength(150)
                .HasColumnName("DESCRICAO");
            builder.Property(s => s.PrecoServico)
                .HasColumnName("PRECO_SERVICO")
                .IsRequired();
            builder.Property(s => s.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
