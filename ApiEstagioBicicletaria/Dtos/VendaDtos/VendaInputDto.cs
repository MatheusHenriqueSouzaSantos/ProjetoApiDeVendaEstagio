using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaInputDto
    {
        [Required(ErrorMessage ="O campo id cliente é obrigatório")]
        public Guid IdCliente { get; set; }
        [Range(0,1000000, ErrorMessage ="O valor do desconto não pode ser negativo")]
        public decimal? Desconto { get; set; } = 0.0m;
        [Required(ErrorMessage = "O campo itensVenda é obrigatório")]
        public List<ItemVendaInputDto> ItensVenda { get; set; }
        [Required(ErrorMessage = "O campo servicosVenda é obrigatório")]
        public List<ServicoVendaInputDto> ServicosVenda { get; set; }

        //public TransacaoDto Transacao { get; set; }

        protected VendaInputDto()
        {

        }

        public VendaInputDto(Guid idCliente, decimal? desconto, List<ProdutoDto> itensVenda, List<ServicoVendaInputDto> servicosVenda)
        {
            IdCliente = idCliente;
            Desconto = desconto;
            //Desconto = desconto ?? 0.0m; fazer isso no service pois se estiver vazio o asp net usa o contrutor sem parametros, se null desconsidera, se diferente considera se não usa o valor
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
            //Transacao = transacaoDto;
        }

        
    }
}
