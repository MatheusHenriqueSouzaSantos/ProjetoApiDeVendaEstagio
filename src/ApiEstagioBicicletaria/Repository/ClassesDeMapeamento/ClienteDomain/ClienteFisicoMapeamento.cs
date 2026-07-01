using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain
{
    public class ClienteFisicoMapeamento : IEntityTypeConfiguration<ClienteFisico>
    {
        public void Configure(EntityTypeBuilder<ClienteFisico> builder)
        {
            builder.ToTable("cliente_fisico");
            //builder.HasKey(c => c.Id);

            //banco já faz isso:

            builder.Property(c => c.Id)
                .IsRequired();

            //builder.HasKey(c => c.Id);
            //builder.Property(c => c.Id)
            //       .ValueGeneratedNever();

            builder.Property(c => c.Nome)
                .HasMaxLength(120)
                .HasColumnName("nome")
                .IsRequired();
            builder.Property(c=>c.Cpf)
                .HasColumnName("cpf")
                .HasMaxLength(11)
                .IsRequired();
        }
    }
}
