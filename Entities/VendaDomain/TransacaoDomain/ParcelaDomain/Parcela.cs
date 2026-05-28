namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain.ParcelaDomain
{
    public class Parcela : EntidadeBase
    {
        public Transacao Transacao { get; private set; }

        public Guid IdTransacao { get; private set; }

        public int NumeroDaParecelaDaVenda { get; set; }

        public decimal ValorParcela { get; set; } = 0.0m;

        public bool Pago { get; set; } = false;


        protected Parcela()
        {

        }

        public Parcela(Transacao transacao, int numeroDaParecelaDaVenda, decimal valorParcela)
        {
            Transacao = transacao;
            NumeroDaParecelaDaVenda = numeroDaParecelaDaVenda;
            ValorParcela = valorParcela;
        }
    }
}
