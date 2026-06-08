using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaInputDto
    {
        [Required(ErrorMessage ="O campo id cliente é obrigatório")]
        public Guid IdCliente { get; set; }
        [Range(0,1000000, ErrorMessage ="O valor do desconto não pode ser negativo")]
        public decimal? DescontoSobreTotalVenda { get; set; } = 0.0m;
        [Required(ErrorMessage = "O campo itens Venda é obrigatório")]
        public List<ItemVendaCreateDto> ItensVenda { get; set; }
        [Required(ErrorMessage = "O campo servicos Venda é obrigatório")]
        public List<ServicoVendaInputDto> ServicosVenda { get; set; }

        [Required(ErrorMessage = "O campo Vendedor Id é obrigatório")]
        public Guid VendedorId { get; private set; }

        protected VendaInputDto()
        {

        }

        public VendaInputDto(Guid idCliente, decimal? descontoSobreTotalVenda, List<ItemVendaCreateDto> itensVenda, List<ServicoVendaInputDto> servicosVenda, Guid vendedorId)
        {
            IdCliente = idCliente;
            DescontoSobreTotalVenda = descontoSobreTotalVenda;
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
            VendedorId = vendedorId;
        }
    }
}
