using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain.EnderecoDomainMaper
{
    public class EnderecoMapeamento : BaseMapeamento<Endereco>
    {
        public override void Configure(EntityTypeBuilder<Endereco> builder) 
        {
            base.Configure(builder);
            builder.ToTable("endereco");

            builder.Property(e => e.Logradouro)
                .HasMaxLength(80)
                .HasColumnName("logradouro")
                .IsRequired();
            builder.Property(e => e.Numero)
                .HasMaxLength(15)
                .HasColumnName("numero")
                .IsRequired();
            builder.Property(e=>e.Cidade)
                .HasMaxLength(35)
                .HasColumnName("cidade")
                .IsRequired();
            builder.Property(e=>e.SiglaUf)
                .HasMaxLength(2)
                .HasColumnName("sigla_uf")
                .IsRequired();
        }

        public void ImplementarValidacao()
        {
            //ignore
            //asgn mt-25
        }
    }
}
