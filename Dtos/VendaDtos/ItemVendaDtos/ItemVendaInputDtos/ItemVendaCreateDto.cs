using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos
{
    public class ItemVendaCreateDto : ItemVendaInputDto
    {

        [Required(ErrorMessage = "O campo id produto é obrigatório")]
        public Guid IdProduto { get; set; }
       

        //public decimal PrecoUnitarioNaVenda { get; set; } = 0.0m;
        //posso pegar pelo Produto e produto pelo id ou manda o preco unitario o front, pois é editavel???

        public ItemVendaCreateDto(Guid idProduto,int quantidade, decimal? descontoUnitario) : base(quantidade, descontoUnitario, TipoItemVendaInputDto.Criacao)
        {
            IdProduto = idProduto;
        }
        
    }
}
