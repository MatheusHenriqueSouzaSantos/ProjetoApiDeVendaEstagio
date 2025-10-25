namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class Parcela
    {
        public Guid Id { get; private set; } = new Guid();

        public Transacao Transacao { get; private set; }

        //coluna necessária?
        public int NumeroDaParecelaDaVenda { get; set; }

        public decimal ValorParcela { get; set; } = 0.0m;

        public bool pago { get; set; } = false;

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
