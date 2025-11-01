namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaTransacaoInputDto
    {
        public VendaInputDto Venda { get; set; }

        public TransacaoInputDto Transacao { get; set; }


        public VendaTransacaoInputDto() { }

        public VendaTransacaoInputDto(VendaInputDto venda, TransacaoInputDto transacao)
        {
            Venda = venda;
            Transacao = transacao;
        }
    }
}
