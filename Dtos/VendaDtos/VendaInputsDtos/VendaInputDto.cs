using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos
{
    public abstract class VendaInputDto
    {
        [Required(ErrorMessage ="O campo id cliente é obrigatório")]
        public Guid IdCliente { get; set; }
        [Range(0,1000000, ErrorMessage ="O valor do desconto não pode ser negativo")]
        public decimal? DescontoSobreTotalVenda { get; set; } = 0.0m;

        [Required(ErrorMessage = "O campo Vendedor Id é obrigatório")]
        public Guid VendedorId { get; private set; }

        protected VendaInputDto()
        {

        }

        protected VendaInputDto(Guid idCliente, decimal? descontoSobreTotalVenda, Guid vendedorId)
        {
            IdCliente = idCliente;
            DescontoSobreTotalVenda = descontoSobreTotalVenda;
            VendedorId = vendedorId;
        }
    }
}
