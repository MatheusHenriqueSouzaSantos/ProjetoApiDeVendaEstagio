using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento
{
    public class BaseLogMapeamento<T> : IEntityTypeConfiguration<T> where T : EntidadeBaseLog
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.Property(t => t.Acao)
                .HasColumnName("ACAO")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.CampoAlterado)
                .HasColumnName("CAMPO_ALTERADO")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.ValorAntigo)
                .HasColumnName("VALOR_ANTIGO")
                .HasMaxLength(300);
            builder.Property(t => t.ValorNovo)
                .HasColumnName("VALOR_NOVO")
                .HasMaxLength(300);
            builder.HasOne(t => t.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(t => t.IdUsuarioResponsavel);
            builder.Property(t => t.IdUsuarioResponsavel)
                .HasColumnName("ID_USUARIO_RESPONSAVEL")
                .HasColumnType("binary(16)")
                .IsRequired();
            builder.Property(t => t.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
        }
    }
}
