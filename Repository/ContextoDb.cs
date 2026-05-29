using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ClienteDomain.EnderecoDomainMaper;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoqueDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EntradaEstoqueDomain.ItemEntradaDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.EstoqueDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.FornecedorDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ProdutoDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.ServicoDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.FinancialTransactionDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.FinancialTransactionDomain.ParcelaDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.ItemVendaDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendaDomain.ServicoVendaDomain;
using ApiEstagioBicicletaria.Repository.ClassesDeMapeamento.VendedorDomain;
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


        public DbSet<ClienteLog> ClienteLogs { get; set; }

        public DbSet<EnderecoLog> EnderecoLogs { get; set; }

        public DbSet<ProdutoLog> ProdutoLogs { get; set; }

        public DbSet<ServicoLog> ServicoLogs { get; set; }

        public DbSet<UsuarioLog> UsuarioLogs { get; set; }

        public DbSet<VendaLog> VendaLog { get; set; }

        public DbSet<TransacaoLog> TransacaoLogs { get; set; }

        public DbSet<ParcelaLog> ParcelaLogs { get; set; }

        public DbSet<ItemVendaLog> ItensVendaLogs { get; set; }

        public DbSet<ServicoVendaLog> ServicosVendaLog { get; set; }

        public DbSet<VendedorLog> VendedorLogs { get; set; }

        public DbSet<FornecedorLog> FornecedorLogs { get; set; }

        public DbSet<EstoqueLog> EstoqueLogs { get; set; }

        public DbSet<EntradaEstoqueLog> EntradasEstoqueLgs { get; set; }

        public DbSet<ItemEntradaEstoqueLog> ItensEntradaEstoqueLogs { get; set; }

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
            modelBuilder.ApplyConfiguration(new ClienteLogMapeamento());
            modelBuilder.ApplyConfiguration(new EnderecoLogMapeamento());
            modelBuilder.ApplyConfiguration(new ItemEntradaEstoqueLogMapeamento());
            modelBuilder.ApplyConfiguration(new EntradaEstoqueLogMapeamento());
            modelBuilder.ApplyConfiguration(new EstoqueLogMapeamento());
            modelBuilder.ApplyConfiguration(new FornecedorLogMapeamento());
            modelBuilder.ApplyConfiguration(new ProdutoLogMapeamento());
            modelBuilder.ApplyConfiguration(new ServicoLogMapeamento());
            modelBuilder.ApplyConfiguration(new UsuarioLogMapeamento());
            modelBuilder.ApplyConfiguration(new ParcelaLogMapeamento());
            modelBuilder.ApplyConfiguration(new TransacaoLogMapeamento());
            modelBuilder.ApplyConfiguration(new ItemVendaLogMapeamento());
            modelBuilder.ApplyConfiguration(new ServicoVendaLogMapeamento());
            modelBuilder.ApplyConfiguration(new VendaLogMapeamento());
            modelBuilder.ApplyConfiguration(new VendedorLogMapeamento());

        }

    }
}
