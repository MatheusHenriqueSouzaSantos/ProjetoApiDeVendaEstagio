using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ItemVendaInputDto
    {
        [Required(ErrorMessage = "O campo id produto é obrigatório")]
        public Guid IdProduto { get; set; }
        [Required(ErrorMessage = "O campo quantidade produto é obrigatório")]
        [Range(0, 1000000, ErrorMessage = "a quantidade de produtos não pode ser negativa")]
        public int Quantidade { get; set; }
        [Range(0, 1000000, ErrorMessage = "O desconto Unitário não pode ser negativo")]
        public decimal? DescontoUnitario { get; set; } = 0.0m;

        //public decimal PrecoUnitarioNaVenda { get; set; } = 0.0m;
        //posso pegar pelo Produto e produto pelo id ou manda o preco unitario o front, pois é editavel???

        public ItemVendaInputDto()
        {

        }

        public ItemVendaInputDto(Guid idProduto, int quantidade, decimal? descontoUnitario)
        {
            IdProduto = idProduto;
            Quantidade = quantidade;
            DescontoUnitario = descontoUnitario;
            //PrecoUnitarioNaVenda = precoUnitarioNaVenda;
        }
    }
}
