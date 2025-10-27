namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain
{
    public class Parcela
    {
        public Guid Id { get; private set; } = new Guid();

        public Transacao Transacao { get; private set; }

        public Guid IdTransacao { get; private set; }

        public DateTime DataCriacao { get; private set; }=DateTime.Now;

        public int NumeroDaParecelaDaVenda { get; set; }

        public decimal ValorParcela { get; set; } = 0.0m;

        public bool Pago { get; set; } = false;

        public bool Ativo { get; set; } = true;

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
