using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.UsuarioDomain
{
    public class UsuarioMapeamento : BaseMapeamento<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);
            builder.ToTable("usuario");

            builder.Property(u => u.CodigoUsuario)
                .HasColumnName("codigo_usuario")
                .HasMaxLength(4)
                .IsRequired();
            builder.Property(u => u.Nome)
                .HasColumnName("nome")
                .HasMaxLength(70)
                .IsRequired();
            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(u=>u.Senha)
                .HasColumnName("senha")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.PerfilUsuario)
                .HasColumnName("perfil_usuario")
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
