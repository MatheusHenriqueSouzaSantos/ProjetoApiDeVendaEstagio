namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaTransacaoOutputDto
    {

        public VendaOutputDto Venda {  get; set; }

        public TransacaoOutputDto Transacao { get; set; }

        protected VendaTransacaoOutputDto()
        {

        }

        public VendaTransacaoOutputDto(VendaOutputDto venda, TransacaoOutputDto transacao)
        {
            Venda = venda;
            Transacao = transacao;
        }
    }
}
