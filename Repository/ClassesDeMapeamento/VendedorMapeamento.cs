using ApiEstagioBicicletaria.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao
{
    public class VendedorMapeamento : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("VENDEDOR");

            builder.HasKey(v=>v.Id);

            builder.Property(v=>v.Id)
                .HasColumnType("Binary(16)")
                .HasColumnName("ID")
                .IsRequired();

            builder.Property(v => v.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(v => v.Telefone)
                .IsRequired()
                .HasColumnName("TELEFONE")
                .HasMaxLength(20);

            builder.Property(v => v.Email)
                .IsRequired()
                .HasColumnName("EMAIL")
                .HasMaxLength(200);

            builder.Property(v => v.NomeCompleto)
                .IsRequired()
                .HasColumnName("NOME_COMPLETO")
                .HasMaxLength(100);

            builder.Property(v => v.Cpf)
                .IsRequired()
                .HasColumnName("CPF")
                .HasMaxLength(11);

            builder.Property(v => v.Ativo)
                .IsRequired()
                .HasColumnName("ATIVO");
        }
    }
}
