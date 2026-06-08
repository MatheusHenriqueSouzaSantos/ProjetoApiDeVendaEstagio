using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos
{
    public abstract class ItemVendaInputDto
    {
        [Required(ErrorMessage = "O campo quantidade produto é obrigatório")]
        [Range(0, 1000000, ErrorMessage = "a quantidade de produtos não pode ser negativa")]
        public int Quantidade { get; private set; }
        [Range(0, 1000000, ErrorMessage = "O desconto Unitário não pode ser negativo")]
        public decimal? DescontoUnitario { get; private set; } = 0.0m;

        public TipoItemVendaInputDto TipoDto { get; private set; }

        protected ItemVendaInputDto(int quantidade, decimal? descontoUnitario, TipoItemVendaInputDto tipoDto)
        {
            Quantidade = quantidade;
            DescontoUnitario = descontoUnitario;
            TipoDto = tipoDto;
        }
    }
}
