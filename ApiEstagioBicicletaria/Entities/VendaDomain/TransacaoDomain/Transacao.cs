namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain
{
    public class Transacao
    {
        public Guid Id { get; private set; } = new Guid();

        public Venda Venda { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public TipoPagamento TipoPagamento { get; set; }

        public MeioPagamaneto MeioPagamaneto { get; set; }

        public bool TransacaoEmCurso { get; set; } = false;

        public bool Pago { get; set; } = false;

        protected Transacao()
        {

        }

        public Transacao(Venda venda, TipoPagamento tipoPagamento, MeioPagamaneto meioPagamaneto)
        {
            Venda = venda;
            TipoPagamento = tipoPagamento;
            MeioPagamaneto = meioPagamaneto;
        }
    }
}
