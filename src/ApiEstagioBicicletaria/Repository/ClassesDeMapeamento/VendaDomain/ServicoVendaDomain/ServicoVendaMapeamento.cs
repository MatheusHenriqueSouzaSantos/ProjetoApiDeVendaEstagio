using ApiEstagioBicicletaria.Entities.VendaDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.ServicoVendaDomain
{
    public class ServicoVendaMapeamento : BaseMapeamento<ServicoVenda>
    {
        public override void Configure(EntityTypeBuilder<ServicoVenda> builder)
        {
            base.Configure(builder);
            builder.ToTable("servico_venda");

            builder.HasOne(s => s.Venda)
                .WithMany()
                .HasForeignKey(s=>s.IdVenda)
                .IsRequired();
            builder.Property(s=>s.IdVenda)
                .HasColumnName("id_venda")
                .IsRequired();
            builder.HasOne(s=>s.Servico)
                .WithMany()
                .HasForeignKey(s=>s.IdServico)
                .IsRequired();
            builder.Property(s => s.IdServico)
                .HasColumnName("id_servico")
                .IsRequired();
            builder.Property(s=>s.DescontoServico)
                .HasColumnName("desconto_servico")
                .IsRequired();
            builder.Property(s=>s.PrecoServicoNaVendaSemDesconto)
                .HasColumnName("preco_servico_na_venda_sem_desconto")
                .IsRequired();
        }
    }
}
