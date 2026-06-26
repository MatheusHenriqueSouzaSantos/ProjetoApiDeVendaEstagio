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
            builder.ToTable("USUARIO");

            builder.Property(u => u.CodigoUsuario)
                .HasColumnName("CODIGO_USUARIO")
                .HasMaxLength(4)
                .IsRequired();
            builder.Property(u => u.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(70)
                .IsRequired();
            builder.Property(u => u.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(u=>u.Senha)
                .HasColumnName("SENHA")
                .HasMaxLength(300)
                .IsRequired();
            builder.Property(u => u.PerfilUsuario)
                .HasColumnName("PERFIL_USUARIO")
                .IsRequired();
        }
    }
}
