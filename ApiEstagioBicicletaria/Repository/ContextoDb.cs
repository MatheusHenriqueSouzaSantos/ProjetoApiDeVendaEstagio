using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeConfiguracao;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Repositories
{
    public class ContextoDb : DbContext
    {

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<ClienteFisico> ClientesFisicos { get; set; }

        public DbSet<ClienteJuridico> ClientesJuridicos { get; set;}

        public DbSet<Endereco> Enderecos { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Servico> Servicos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public ContextoDb(DbContextOptions<ContextoDb> options) : base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
            .ToTable("cliente");

            modelBuilder.Entity<ClienteFisico>()
                .ToTable("cliente_fisico")
                .HasBaseType<Cliente>(); 

            modelBuilder.Entity<ClienteJuridico>()
                .ToTable("cliente_juridico")
                .HasBaseType<Cliente>(); 

            modelBuilder.ApplyConfiguration(new ClienteConfiguracao());
            modelBuilder.ApplyConfiguration(new ClienteFisicoConfiguracao());
            modelBuilder.ApplyConfiguration(new ClienteJuridicoConfiguracao());
            modelBuilder.ApplyConfiguration(new EnderecoConfiguracao());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguracao());
            modelBuilder.ApplyConfiguration(new ServicoConfiguracao());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracao());
        }

    }
}
