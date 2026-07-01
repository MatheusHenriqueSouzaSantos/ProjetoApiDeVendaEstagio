using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos
{
    public class ItemVendaUpdateDto
    {

        [Range(0, 1000000, ErrorMessage = "a quantidade de produtos não pode ser negativa")]
        public int? Quantidade { get; set; }
        [Range(0, 1000000, ErrorMessage = "O desconto Unitário não pode ser negativo")]
        public decimal? DescontoUnitario { get; set; } = 0.0m;

        [Required(ErrorMessage = "O campo id item é obrigatório")]
        public Guid IdItem { get; set; }

        public ItemVendaUpdateDto(int? quantidade, decimal? descontoUnitario, Guid idItem)
        {
            Quantidade = quantidade;
            DescontoUnitario = descontoUnitario;
            IdItem = idItem;
        }

        public ItemVendaUpdateDto()
        {

        }
    }
}
