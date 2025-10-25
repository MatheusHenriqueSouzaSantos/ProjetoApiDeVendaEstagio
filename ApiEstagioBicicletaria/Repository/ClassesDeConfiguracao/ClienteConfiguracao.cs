using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class ClienteConfiguracao : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("cliente");

            builder.HasKey(c=>c.Id);

            builder.Property(c => c.Id)
                .HasColumnType("Binary(16)")
                .HasColumnName("ID")
                .IsRequired();

            //duvidaEnderecoTemMaisDeUmCliente
            builder.HasOne(c => c.Endereco)
                .WithMany()
                .HasForeignKey("ID_ENDERECO")
                .IsRequired();
            builder.Property(c => c.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(c => c.Telefone)
                .HasMaxLength(2)
                .HasColumnName("TELEFONE")
                .IsRequired();
            //email até 254
            builder.Property(c=>c.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL")
                .IsRequired();
            builder.Property(c=>c.TipoCliente)
                .HasConversion<string>()
                .HasMaxLength(25)
                .HasColumnName("TIPO_CLIENTE")
                .IsRequired();
            builder.Property(c=>c.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();


        }
    }
}
