using ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain
{
    public class Transacao : EntidadeBase
    {
        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Venda Venda { get; private set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid IdVenda { get; private set; }

        public TipoPagamento TipoPagamento { get; set; }

        public MeioPagamento MeioPagamento { get; set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public bool TransacaoEmCurso { get; set; } = false;

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public bool Pago { get; set; } = false;


        protected Transacao()
        {

        }

        public Transacao(Venda venda, TipoPagamento tipoPagamento, MeioPagamento meioPagamaneto)
        {
            Venda = venda;
            IdVenda = venda.Id;
            TipoPagamento = tipoPagamento;
            MeioPagamento = meioPagamaneto;
        }

        public Transacao Copia()
        {
            return new Transacao(Venda, TipoPagamento, MeioPagamento);
        }
    }
}
