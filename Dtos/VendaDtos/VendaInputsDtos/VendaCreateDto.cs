using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos
{
    public class VendaCreateDto 
    {

        [Required(ErrorMessage = "O campo id cliente é obrigatório")]
        public Guid IdCliente { get; set; }
        [Range(0, 1000000, ErrorMessage = "O valor do desconto não pode ser negativo")]
        public decimal? DescontoSobreTotalVenda { get; set; } = 0.0m;

        [Required(ErrorMessage = "O campo Vendedor Id é obrigatório")]
        public Guid VendedorId { get; private set; }

        [Required(ErrorMessage = "O campo itens Venda é obrigatório")]
        public List<ItemVendaCreateDto> ItensVenda { get; set; }
        [Required(ErrorMessage = "O campo servicos Venda é obrigatório")]
        public List<ServicoVendaCreateDto> ServicosVenda { get; set; }

        public VendaCreateDto(Guid idCliente, decimal? descontoSobreTotalVenda, Guid vendedorId, List<ItemVendaCreateDto> itensVenda, 
            List<ServicoVendaCreateDto> servicosVenda)
        {
            IdCliente = idCliente;
            DescontoSobreTotalVenda = descontoSobreTotalVenda;
            VendedorId = vendedorId;
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
        }

        public VendaCreateDto()
        {
        }


    }
}
