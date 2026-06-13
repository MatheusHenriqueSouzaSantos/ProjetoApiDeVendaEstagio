using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos
{
    public class ItemVendaCreateDto : ItemVendaInputDto
    {

        [Required(ErrorMessage = "O campo id produto é obrigatório")]
        public Guid IdProduto { get; set; }
       

        public ItemVendaCreateDto(Guid idProduto,int quantidade, decimal? descontoUnitario) : base(quantidade, descontoUnitario)
        {
            IdProduto = idProduto;
        }

        public ItemVendaCreateDto()
        {

        }
    }
}
