using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.Venda
{
    public class ServicoVendaMapeamento : BaseMapeamento<ServicoVenda>
    {
        public override void Configure(EntityTypeBuilder<ServicoVenda> builder)
        {
            base.Configure(builder);
            builder.ToTable("SERVICO_VENDA");

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
            builder.Property(s=>s.DescontoServico)
                .HasColumnName("DESCONTO_SERVICO")
                .IsRequired();
            builder.Property(s=>s.PrecoServicoNaVendaSemDesconto)
                .HasColumnName("PRECO_SERVICO_NA_VENDA_SEM_DESCONTO")
                .IsRequired();
        }
    }
}
