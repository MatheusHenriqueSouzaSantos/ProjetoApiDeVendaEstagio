using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class EnderecoMapeamento : BaseMapeamento<Endereco>
    {
        public override void Configure(EntityTypeBuilder<Endereco> builder) 
        {
            base.Configure(builder);
            builder.ToTable("ENDERECO");

            builder.Property(e => e.Logradouro)
                .HasMaxLength(80)
                .HasColumnName("LOGRADOURO")
                .IsRequired();
            builder.Property(e => e.Numero)
                .HasMaxLength(15)
                .HasColumnName("NUMERO")
                .IsRequired();
            builder.Property(e=>e.Cidade)
                .HasMaxLength(35)
                .HasColumnName("CIDADE")
                .IsRequired();
            builder.Property(e=>e.SiglaUf)
                .HasMaxLength(2)
                .HasColumnName("SIGLA_UF")
                .IsRequired();
        }

        public void ImplementarValidacao()
        {
            //ignore
            //asgn mt-25
        }
    }
}
