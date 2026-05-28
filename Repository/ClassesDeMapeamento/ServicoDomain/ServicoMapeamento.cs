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
            builder.ToTable("SERVICO");
      
            builder.Property(s => s.CodigoDoServico)
                .HasColumnName("CODIGO_DO_SERVICO")
                .HasMaxLength(128)
                .IsRequired();
            builder.Property(s => s.NomeServico)
                .HasColumnName("NOME_SERVICO")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(s => s.Descricao)
                .HasMaxLength(150)
                .HasColumnName("DESCRICAO");
            builder.Property(s => s.Preco)
                .HasColumnName("PRECO_SERVICO")
                .IsRequired();
        }
    }
}
