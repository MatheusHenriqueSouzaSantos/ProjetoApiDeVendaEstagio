using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaOutputDto
    {
        public Guid IdVenda { get; set; }

        public DateTime DataCriacao { get; set; }

        public decimal Desconto { get; set; }

        public decimal ValorTotalComDescontoAplicado { get; set; }

        public Cliente Cliente { get; set; }
        
        public List<ItemVendaOutputDto> ItensVenda { get; set; }
        public List<ServicoVendaOutputDto> ServicosVenda { get; set; }

        protected VendaOutputDto()
        {

        }

        public VendaOutputDto(Guid idVenda, DateTime dataCriacao, decimal desconto, decimal valorTotal, Cliente cliente, List<ItemVendaOutputDto> itensVenda, List<ServicoVendaOutputDto> servicosVenda)
        {
            IdVenda = idVenda;
            DataCriacao = dataCriacao;
            Desconto = desconto;
            ValorTotalComDescontoAplicado = valorTotal;
            Cliente = cliente;
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
        }
    }
}
