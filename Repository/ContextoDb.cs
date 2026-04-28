using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
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

        public DbSet<Venda> Vendas { get; set; }

        public DbSet<Transacao> Transacoes { get; set; }

        public DbSet<Parcela> Parcelas { get; set; }

        public DbSet<ItemVenda> ItensVendas { get; set; }

        public DbSet<ServicoVenda> ServicosVendas { get; set; }

        public DbSet<Vendedor> Vendedores { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set; }

        public DbSet<Estoque> Estoques { get; set; }

        public DbSet<EntradaEstoque> EntradasEstoque { get; set; }

        public DbSet<ItemEntradaEstoque> ItensEntradaEstoque {get; set; }

        public ContextoDb(DbContextOptions<ContextoDb> options) : base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Cliente>()
            .ToTable("CLIENTE");

            modelBuilder.Entity<ClienteFisico>()
                .ToTable("CLIENTE_FISICO")
                .HasBaseType<Cliente>(); 

            modelBuilder.Entity<ClienteJuridico>()
                .ToTable("CLIENTE_JURIDICO")
                .HasBaseType<Cliente>(); 

            modelBuilder.ApplyConfiguration(new ClienteMapeamento());
            modelBuilder.ApplyConfiguration(new ClienteFisicoMapeamento());
            modelBuilder.ApplyConfiguration(new ClienteJuridicoMapeamento());
            modelBuilder.ApplyConfiguration(new EnderecoMapeamento());
            modelBuilder.ApplyConfiguration(new ProdutoMapeamento());
            modelBuilder.ApplyConfiguration(new ServicoMapeamento());
            modelBuilder.ApplyConfiguration(new UsuarioMapeamento());
            modelBuilder.ApplyConfiguration(new VendaMapeamento());
            modelBuilder.ApplyConfiguration(new TransacaoMapeamento());
            modelBuilder.ApplyConfiguration(new ParcelaMapeamento());
            modelBuilder.ApplyConfiguration(new ItemVendaMapeamento());
            modelBuilder.ApplyConfiguration(new ServicoVendaMapeamento());
            modelBuilder.ApplyConfiguration(new VendedorMapeamento());
            modelBuilder.ApplyConfiguration(new FornecedorMapeamento());
            modelBuilder.ApplyConfiguration(new EstoqueMapeamento());
            modelBuilder.ApplyConfiguration(new EntradaEstoqueMapeamento());
            modelBuilder.ApplyConfiguration(new ItemEntradaEstoqueMapeamento());
        }

    }
}
