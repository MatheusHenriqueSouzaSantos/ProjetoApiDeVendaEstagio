using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.UsuarioDomain
{
    public class UsuarioLogMapeamento : BaseLogMapeamento<UsuarioLog>
    {
        public override void Configure(EntityTypeBuilder<UsuarioLog> builder)
        {
            base.Configure(builder);
            builder.ToTable("LOG_USUARIO");

            builder.HasOne(u => u.Usuario)
                .WithMany()
                .HasForeignKey(u => u.IdUsuario)
                .IsRequired();
            builder.Property(u => u.IdUsuario)
                .HasColumnType("binary(16)")
                .IsRequired();
        }
    }
}
