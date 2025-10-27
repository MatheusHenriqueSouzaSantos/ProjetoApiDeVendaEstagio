using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class Venda
    {
        public Guid Id { get; private set; } = new Guid();
        public Cliente Cliente { get;  set; }

        public Guid IdCliente { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public decimal Desconto { get; set; } = 0.0m;
        //pegar o valor total pela soma de todos os itens menos o desconto
        public decimal ValorTotal { get; set; } = 0.0m;

        public bool Ativo { get; set; } = true;

        protected Venda()
        {

        }

        public Venda(Cliente cliente, decimal desconto, decimal valorTotal)
        {
            Cliente = cliente;
            Desconto = desconto;
            ValorTotal = valorTotal;
        }
    }
}
