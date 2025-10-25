using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ClienteFisicoConfiguracao : IEntityTypeConfiguration<ClienteFisico>
    {
        public void Configure(EntityTypeBuilder<ClienteFisico> builder)
        {
            builder.ToTable("cliente_fisico");
            //builder.HasKey(c => c.Id);

            //banco já faz isso:

            //builder.Property(c => c.Id)
            //    .HasColumnType("Binary(16)")
            //    .IsRequired();

            builder.Property(c => c.Nome)
                .HasMaxLength(70)
                .HasColumnName("NOME")
                .IsRequired();
            builder.Property(c=>c.Cpf)
                .HasColumnName("CPF")
                .HasMaxLength(11)
                .IsRequired();
        }
    }
}
