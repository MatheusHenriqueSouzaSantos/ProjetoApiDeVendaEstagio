using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteMapeamentoDomain
{
    public class ClienteMapeamento : BaseMapeamento<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            base.Configure(builder);
            builder.ToTable("CLIENTE");

            //duvidaEnderecoTemMaisDeUmCliente
            builder.HasOne(c => c.Endereco)
                .WithMany()
                .HasForeignKey("ID_ENDERECO")
                .IsRequired();
            builder.Property(c => c.Telefone)
                .HasMaxLength(20)
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


        }
    }
}
