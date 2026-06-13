using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos
{
    public class ItemVendaUpdateDto : ItemVendaInputDto
    {

        [Required(ErrorMessage = "O campo id item é obrigatório")]
        public Guid IdItem { get; set; }

        public ItemVendaUpdateDto(Guid idItem,int quantidade, decimal? descontoUnitario) : base(quantidade, descontoUnitario)
        {
            IdItem = idItem;
        }

        public ItemVendaUpdateDto()
        {

        }
    }
}
