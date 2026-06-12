using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos
{
    public class ServicoVendaCreateDto : ServicoVendaInputDto
    {
        [Required(ErrorMessage = "O campo id servico é obrigatório")]
        public Guid IdServico { get; set; }
        public ServicoVendaCreateDto()
        {
        }

        public ServicoVendaCreateDto(Guid idServico,decimal? descontoServico) : base(descontoServico)
        {
            IdServico = idServico;
        }



    }
}
