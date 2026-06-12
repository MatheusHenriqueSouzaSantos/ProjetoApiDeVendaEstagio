using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos
{
    public class ServicoVendaInputDto
    {
        [Range(0, 1000000, ErrorMessage = "O desconto do Serviço não pode ser negativo")]
        public decimal? DescontoServico { get; set; } = 0.0m;

        //public decimal PrecoServicoNaVenda { get; set; } = 0.0m;
        //posso pegar pelo serviço e produto pelo id ou manda o preco unitario o front, pois é editavel???

        public ServicoVendaInputDto()
        {

        }

        public ServicoVendaInputDto(decimal? descontoServico)
        {
            DescontoServico = descontoServico;
        }
    }
    
}
