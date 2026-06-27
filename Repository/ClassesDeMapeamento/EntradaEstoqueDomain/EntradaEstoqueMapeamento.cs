using ApiEstagioBicicletaria;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoqueDomain
{ 
    public class EntradaEstoqueMapeamento : BaseMapeamento<EntradaEstoque>
    {
        public override void Configure(EntityTypeBuilder<EntradaEstoque> builder)
        {
            base.Configure(builder);

            builder.ToTable("entrada_estoque");

            builder.HasOne(e=>e.Fornecedor)
                .WithMany()
                .HasForeignKey(e=>e.IdFornecedor)
                .IsRequired();
            builder.Property(e=>e.IdFornecedor)
                .HasColumnName("id_fornecedor")
                .IsRequired();
            builder.Property(e=>e.CodigoEntrada)
                .HasColumnName("codigo_entrada")
                .IsRequired();
            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();
        
        }
    }

}

