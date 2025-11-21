using ApiEstagioBicicletaria.Entities.ClienteDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class Venda
    {
        public Guid Id { get; private set; } = Guid.NewGuid(); 
        public int CodigoVenda { get; private set; }
        public Cliente Cliente { get;  set; }

        public Guid IdCliente { get;  set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public decimal DescontoTotal { get; set; } = 0.0m;
        //pegar o valor total pela soma de todos os itens menos o desconto

        public decimal ValorTotalSemDesconto { get; set; } = 0.0m;

        public decimal ValorTotalComDesconto { get; set; } = 0.0m;

        public bool Ativo { get; set; } = true;

        protected Venda()
        {

        }

        public Venda(Cliente cliente, Guid idCliente, decimal descontoTotal,decimal valorTotalSemDesconto, decimal valorTotalComDesconto)
        {
            Cliente = cliente ;
            IdCliente = idCliente;
            DescontoTotal = descontoTotal;
            ValorTotalSemDesconto = valorTotalSemDesconto;
            ValorTotalComDesconto = valorTotalComDesconto;
        }



        //public Venda(Cliente cliente, decimal desconto, decimal valorTotal)
        //{
        //    Cliente = cliente;
        //    Desconto = desconto;
        //    ValorTotal = valorTotal;
        //}
    }
}
