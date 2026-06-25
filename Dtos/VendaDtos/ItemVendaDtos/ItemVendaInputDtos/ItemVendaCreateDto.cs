using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos
{
    public class ItemVendaCreateDto
    {
        [Required(ErrorMessage = "O campo quantidade produto é obrigatório")]
        [Range(1, 1000000, ErrorMessage = "a quantidade de produtos deve ser maior que 0")]
        public int Quantidade { get; set; }
        [Range(0, 1000000, ErrorMessage = "O desconto Unitário não pode ser negativo")]
        public decimal? DescontoUnitario { get; set; } = 0.0m;

        [Required(ErrorMessage = "O campo id produto é obrigatório")]
        public Guid IdProduto { get; set; }

        public ItemVendaCreateDto(int quantidade, decimal? descontoUnitario, Guid idProduto)
        {
            Quantidade = quantidade;
            DescontoUnitario = descontoUnitario;
            IdProduto = idProduto;
        }

        public ItemVendaCreateDto()
        {

        }
    }
}
