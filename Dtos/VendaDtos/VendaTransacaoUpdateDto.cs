using ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaTransacaoUpdateDto
    {
        public VendaUpdateDto Venda { get; set; }

        public TransacaoInputDto Transacao { get; set; }


        public VendaTransacaoUpdateDto() { }

        public VendaTransacaoUpdateDto(VendaUpdateDto venda, TransacaoInputDto transacao)
        {
            Venda = venda;
            Transacao = transacao;
        }
    }
}
