using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class ServicoVendaInputDto
    {
        [Required(ErrorMessage = "O campo id servico é obrigatório")]
        public Guid IdServico { get; set; }
        [Range(0, 1000000, ErrorMessage = "O desconto do Serviço não pode ser negativo")]
        public decimal? DescontoServico { get; set; } = 0.0m;

        //public decimal PrecoServicoNaVenda { get; set; } = 0.0m;
        //posso pegar pelo serviço e produto pelo id ou manda o preco unitario o front, pois é editavel???

        public ServicoVendaInputDto()
        {

        }

        public ServicoVendaInputDto(Guid idServico, decimal? descontoServico)
        {
            IdServico = idServico;
            DescontoServico = descontoServico;
            //PrecoServicoNaVenda = precoServicoNaVenda;
        }
    }
    
}
