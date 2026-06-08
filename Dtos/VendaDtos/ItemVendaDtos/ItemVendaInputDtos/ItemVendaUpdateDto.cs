using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos
{
    public class ItemVendaUpdateDto : ItemVendaInputDto
    {

        [Required(ErrorMessage = "O campo id item é obrigatório")]
        public Guid IdItem { get; set; }

        //public decimal PrecoUnitarioNaVenda { get; set; } = 0.0m;
        //posso pegar pelo Produto e produto pelo id ou manda o preco unitario o front, pois é editavel???
        public ItemVendaUpdateDto(Guid idItem,int quantidade, decimal? descontoUnitario) : base(quantidade, descontoUnitario, TipoItemVendaInputDto.Atualizacao)
        {
            IdItem = idItem;
        }

        
    }
}
