using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class Venda
    {
        //ter só a transação como objeto ou ter o id da transação para ajudar?
        public Guid Id { get; private set; } = new Guid();
        //nunca permitir alterar o cliente de uma venda
        public Cliente Cliente { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;
        //nao deixar adicionar um desconto como null
        public decimal Desconto { get; set; } = 0.0m;

        public decimal ValorTotalVenda { get; set; } = 0.0m;
        //venda aberta?
        public bool Ativo { get; set; } = true;

        protected Venda()
        {

        }

        public Venda(Cliente cliente, decimal desconto, decimal valorTotalVenda)
        {
            Cliente = cliente;
            Desconto = desconto;
            ValorTotalVenda = valorTotalVenda;
        }
    }
}
