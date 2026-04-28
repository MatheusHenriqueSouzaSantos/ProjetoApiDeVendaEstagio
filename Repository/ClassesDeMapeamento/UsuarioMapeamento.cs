using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class UsuarioMapeamento : BaseMapeamento<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);
            builder.ToTable("USUARIO");

            builder.Property(u => u.Email)
                .HasColumnName("EMAIL")
                .IsRequired();
            builder.Property(u=>u.Senha)
                .HasColumnName("SENHA")
                .IsRequired();
        }
    }
}
