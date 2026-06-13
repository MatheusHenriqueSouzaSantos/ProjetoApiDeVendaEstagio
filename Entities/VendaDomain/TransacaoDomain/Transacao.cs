using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain
{
    public class Transacao : EntidadeBase
    {

        public Venda Venda { get; private set; }

        public Guid IdVenda { get; private set; }

        public TipoPagamento TipoPagamento { get; set; }

        public MeioPagamento MeioPagamento { get; set; }

        public bool TransacaoEmCurso { get; set; } = false;

        public bool Pago { get; set; } = false;


        protected Transacao()
        {

        }

        public Transacao(Venda venda, TipoPagamento tipoPagamento, MeioPagamento meioPagamaneto)
        {
            Venda = venda;
            this.TipoPagamento = tipoPagamento;
            this.MeioPagamento = meioPagamaneto;
        }
    }
}
