using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos
{
    public class VendaCreateDto : VendaInputDto
    {
        [Required(ErrorMessage = "O campo itens Venda é obrigatório")]
        public List<ItemVendaCreateDto> ItensVenda { get; set; }
        [Required(ErrorMessage = "O campo servicos Venda é obrigatório")]
        public List<ServicoVendaCreateDto> ServicosVenda { get; set; }
    }
}
