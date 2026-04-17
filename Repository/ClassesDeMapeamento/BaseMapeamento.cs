using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria;

public class BaseMapeamento<T> : IEntityTypeConfiguration<T> where T: EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {

         builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnType("binary(16)")
                .HasColumnName("ID")
                .IsRequired();
            builder.Property(t => t.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
            builder.Property(t => t.Ativo)
                .HasColumnName("ATIVO")
                .IsRequired();
    }
}
