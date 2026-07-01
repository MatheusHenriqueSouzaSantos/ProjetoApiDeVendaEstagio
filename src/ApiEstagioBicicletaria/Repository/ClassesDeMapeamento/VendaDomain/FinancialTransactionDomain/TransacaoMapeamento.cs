using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.FinancialTransactionDomain
{
    public class TransacaoMapeamento : BaseMapeamento<Transacao>
    {
        public override void Configure(EntityTypeBuilder<Transacao> builder)
        {
            base.Configure(builder);

            builder.ToTable("transacao");

            builder.HasOne(t => t.Venda)
                .WithOne()
                .HasForeignKey<Transacao>(t=>t.IdVenda)
                .IsRequired();
            builder.Property(t => t.IdVenda)
                .HasColumnName("id_venda")
                .IsRequired();
            builder.Property(t => t.TipoPagamento)
                .HasColumnName("tipo_pagamento")
                .HasConversion<string>()
                .IsRequired();
            builder.Property(t => t.MeioPagamento)
                .HasColumnName("meio_pagamento")
                .HasConversion<string>()
                .IsRequired();
            builder.Property(t => t.TransacaoEmCurso)
                .HasColumnName("transacao_em_curso")
                .IsRequired();
            builder.Property(t => t.Pago)
                .HasColumnName("pago")
                .IsRequired();
        }
    }
}
