using ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaTransacaoCreateDto
    {
        public VendaCreateDto Venda { get; set; }

        public TransacaoCreateDto Transacao { get; set; }


        public VendaTransacaoCreateDto() { }

        public VendaTransacaoCreateDto(VendaCreateDto venda, TransacaoCreateDto transacao)
        {
            Venda = venda;
            Transacao = transacao;
        }
    }
}
