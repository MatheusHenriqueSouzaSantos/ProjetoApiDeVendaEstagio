namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaTransacaoDto
    {
        public VendaDto Venda { get; set; }

        public TransacaoDto Transacao { get; set; }


        public VendaTransacaoDto() { }

        public VendaTransacaoDto(VendaDto venda, TransacaoDto transacao)
        {
            Venda = venda;
            Transacao = transacao;
        }
    }
}
