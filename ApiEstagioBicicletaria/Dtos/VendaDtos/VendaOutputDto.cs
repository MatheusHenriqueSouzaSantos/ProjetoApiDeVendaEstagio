using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaOutputDto
    {
        public Guid IdVenda { get; set; }

        public DateTime DataCriacao { get; set; }

        public decimal? Desconto { get; set; }

        public decimal ValorTotal { get; set; }

        public Cliente Cliente { get; set; }
        
        public List<ItemVenda> ItensVenda { get; set; }
        public List<ServicoVenda> ServicosVenda { get; set; }

        protected VendaOutputDto()
        {

        }

        public VendaOutputDto(Guid idVenda, DateTime dataCriacao, decimal? desconto, decimal valorTotal, Cliente cliente, List<ItemVenda> itensVenda, List<ServicoVenda> servicosVenda)
        {
            IdVenda = idVenda;
            DataCriacao = dataCriacao;
            Desconto = desconto;
            ValorTotal = valorTotal;
            Cliente = cliente;
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
        }
    }
}
