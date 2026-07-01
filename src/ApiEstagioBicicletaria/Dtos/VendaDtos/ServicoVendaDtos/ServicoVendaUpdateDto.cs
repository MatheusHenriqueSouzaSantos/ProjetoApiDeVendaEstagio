using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos
{
    public class ServicoVendaUpdateDto : ServicoVendaInputDto
    {
        [Required(ErrorMessage = "O campo id servico da venda é obrigatório")]
        public Guid IdServicoVenda { get; set; }
        public ServicoVendaUpdateDto()
        {
        }

        public ServicoVendaUpdateDto(Guid idServicoVenda,decimal? descontoServico) : base(descontoServico)
        {
            IdServicoVenda = idServicoVenda;
        }




    }
}
