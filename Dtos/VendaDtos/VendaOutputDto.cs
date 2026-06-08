using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaOutputDto
    {
        public Guid IdVenda { get; private set; }

        public string CodigoVenda { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public decimal DescontoTotalVenda { get; private set; }

        public decimal ValorTotalSemDesconto { get; private set; }

        public decimal ValorTotalComDesconto { get; private set; }

        public Cliente Cliente { get; private set; }
        
        public List<ItemVendaOutputDto> ItensVenda { get; private set; }
        public List<ServicoVendaOutputDto> ServicosVenda { get; private set; }

        public Vendedor Vendedor { get; private set; }

        protected VendaOutputDto()
        {

        }

        public VendaOutputDto(Guid idVenda, string codigoVenda, DateTime dataCriacao, 
            decimal descontoTotalVenda, decimal valorTotalSemDesconto, decimal valorTotalComDesconto, 
            Cliente cliente, List<ItemVendaOutputDto> itensVenda, List<ServicoVendaOutputDto> servicosVenda, Vendedor vendedor)
        {
            IdVenda = idVenda;
            CodigoVenda = codigoVenda;
            DataCriacao = dataCriacao;
            DescontoTotalVenda = descontoTotalVenda;
            ValorTotalSemDesconto = valorTotalSemDesconto;
            ValorTotalComDesconto = valorTotalComDesconto;
            Cliente = cliente;
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
            Vendedor = vendedor;
        }
    }
}
