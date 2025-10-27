using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ServicoVendaConfiguracao : IEntityTypeConfiguration<ServicoVenda>
    {
        public void Configure(EntityTypeBuilder<ServicoVenda> builder)
        {
            builder.ToTable("servico_venda");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.HasOne(s => s.Venda)
                .WithMany()
                .HasForeignKey(s=>s.IdVenda)
                .IsRequired();
            builder.Property(s=>s.IdVenda)
                .HasColumnType("binary(16)")
                .HasColumnName("ID_VENDA")
                .IsRequired();
            builder.HasOne(s=>s.Servico)
                .WithMany()
                .HasForeignKey(s=>s.IdServico)
                .IsRequired();
            builder.Property(s => s.IdServico)
                .HasColumnName("ID_SERVICO")
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.Property(s=>s.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(s=>s.DescontoServico)
                .HasColumnName("DESCONTO_SERVICO")
                .IsRequired();
            builder.Property(i=>i.PrecoServicoNaVenda)
                .HasColumnName("PRECO_SERVICO_NA_VENDA")
                .IsRequired();
            builder.Property(s=>s.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
        }
    }
}
