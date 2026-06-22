namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain
{
    public class Parcela : EntidadeBase
    {
        public Transacao Transacao { get; private set; }

        public Guid IdTransacao { get; private set; }

        public int NumeroDaParcelaDaVenda { get; set; }

        public decimal ValorParcela { get; set; } = 0.0m;

        public bool Pago { get; set; } = false;

        public DateOnly DataVencimento { get; set; }


        protected Parcela()
        {

        }

        public Parcela(Transacao transacao, int numeroDaParecelaDaVenda, decimal valorParcela, DateOnly dataVencimento)
        {
            Transacao = transacao;
            NumeroDaParcelaDaVenda = numeroDaParecelaDaVenda;
            ValorParcela = valorParcela;
            DataVencimento = dataVencimento;
        }
    }
}
