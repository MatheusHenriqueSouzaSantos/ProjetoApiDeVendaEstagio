using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos
{
    public class VendaCreateDto : VendaInputDto
    {
        public VendaCreateDto()
        {
        }


        [Required(ErrorMessage = "O campo itens Venda é obrigatório")]
        public List<ItemVendaCreateDto> ItensVenda { get; set; }
        [Required(ErrorMessage = "O campo servicos Venda é obrigatório")]
        public List<ServicoVendaCreateDto> ServicosVenda { get; set; }
        public VendaCreateDto(Guid idCliente, decimal? descontoSobreTotalVenda, Guid vendedorId,List<ItemVendaCreateDto> itensVenda,List<ServicoVendaCreateDto> servicosVenda) : base(idCliente, descontoSobreTotalVenda, vendedorId)
        {
            ItensVenda = itensVenda;
            ServicosVenda = servicosVenda;
        }


    }
}
