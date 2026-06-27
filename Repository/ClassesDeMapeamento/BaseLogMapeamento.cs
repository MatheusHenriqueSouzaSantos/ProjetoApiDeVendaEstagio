using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento
{
    public class BaseLogMapeamento<T> : IEntityTypeConfiguration<T> where T : EntidadeBaseLog
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
           v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
           v => DateTime.SpecifyKind(v, DateTimeKind.Local).ToLocalTime()
       );

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .IsRequired();
            builder.Property(t => t.Acao)
                .HasColumnName("acao")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.CampoAlterado)
                .HasColumnName("campo_alterado")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.ValorAntigo)
                .HasColumnName("valor_antigo")
                .HasMaxLength(300);
            builder.Property(t => t.ValorNovo)
                .HasColumnName("valor_novo")
                .HasMaxLength(300);
            builder.HasOne(t => t.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(t => t.IdUsuarioResponsavel);
            builder.Property(t => t.IdUsuarioResponsavel)
                .HasColumnName("id_usuario_responsavel")
                .IsRequired();
            builder.Property(t => t.DataCriacao)
                .HasColumnName("data_criacao")
                .HasConversion(dateTimeConverter)
                .IsRequired();
        }
    }
}
