using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos
{
    public class VendaUpdateDto: VendaInputDto
    {
        [Required(ErrorMessage = "O campo itens Venda novos é obrigatório")]
        public List<ItemVendaCreateDto> ItensVendaNovos { get; set; }
        [Required(ErrorMessage = "O campo servicos Venda novos é obrigatório")]
        public List<ServicoVendaCreateDto> ServicosVendaNovos { get; set; }

        [Required(ErrorMessage = "O campo itens Venda atualizados é obrigatório")]
        public List<ItemVendaUpdateDto> ItensVendaAtualizados { get; set; }
        [Required(ErrorMessage = "O campo servicos Venda atualizados é obrigatório")]
        public List<ServicoVendaUpdateDto> ServicosVendaAtualizados { get; set; }

        public VendaUpdateDto()
        {
        }

        public VendaUpdateDto(List<ItemVendaCreateDto> itensVendaNovos, List<ServicoVendaCreateDto> servicosVendaNovos, List<ItemVendaUpdateDto> itensVendaAtualizados, 
            List<ServicoVendaUpdateDto> servicosVendaAtualizados, Guid idCliente, decimal? descontoSobreTotalVenda, Guid vendedorId) : base(idCliente, descontoSobreTotalVenda, vendedorId)
        {
            ItensVendaNovos = itensVendaNovos;
            ServicosVendaNovos = servicosVendaNovos;
            ItensVendaAtualizados = itensVendaAtualizados;
            ServicosVendaAtualizados = servicosVendaAtualizados;
        }


    }
}
