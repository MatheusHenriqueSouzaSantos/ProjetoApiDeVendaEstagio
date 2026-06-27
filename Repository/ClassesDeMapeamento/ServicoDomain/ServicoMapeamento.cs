using ApiEstagioBicicletaria.Entities.ServicoDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ServicoDomain
{
    public class ServicoMapeamento : BaseMapeamento<Servico>
    {
        public override void Configure(EntityTypeBuilder<Servico> builder)
        {
            base.Configure(builder);
            builder.ToTable("servico");
      
            builder.Property(s => s.CodigoDoServico)
                .HasColumnName("codigo_do_servico")
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(s => s.NomeServico)
                .HasColumnName("nome_servico")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(s => s.Descricao)
                .HasMaxLength(150)
                .HasColumnName("descricao");
            builder.Property(s => s.Preco)
                .HasColumnName("preco_servico")
                .IsRequired();
        }
    }
}
