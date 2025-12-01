using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaOutputDto
    {
        public Guid IdVenda { get; set; }

        public string CodigoVenda { get; set; }

        public DateTime DataCriacao { get; set; }

        public decimal DescontoTotalVenda { get; set; }

        public decimal ValorTotalSemDesconto { get; set; }

        public decimal ValorTotalComDesconto { get; set; }

        public Cliente Cliente { get; set; }
        
        public List<ItemVendaOutputDto> ItensVenda { get; set; }
        public List<ServicoVendaOutputDto> ServicosVenda { get; set; }

        protected VendaOutputDto()
        {

        }

        public VendaOutputDto(Guid idVenda, string codigoVenda, DateTime dataCriacao, decimal descontoTotalVenda,decimal valorTotalSemDesconto, decimal valorTotalComDesconto, Cliente cliente, List<ItemVendaOutputDto> itensVenda, List<ServicoVendaOutputDto> servicosVenda)
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
        }
    }
}
