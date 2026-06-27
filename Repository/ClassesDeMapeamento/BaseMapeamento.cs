using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiEstagioBicicletaria;

public class BaseMapeamento<T> : IEntityTypeConfiguration<T> where T: EntidadeBase
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
            builder.Property(t => t.DataCriacao)
                .HasColumnName("data_criacao")
                .HasConversion(dateTimeConverter)
                .IsRequired();
            builder.Property(t => t.Ativo)
                .HasColumnName("ativo")
                .IsRequired();
    }
}
