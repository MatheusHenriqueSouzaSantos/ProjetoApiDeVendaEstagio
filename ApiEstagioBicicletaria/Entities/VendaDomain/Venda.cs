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

        public decimal ValorTotalSemDescontoAplicado { get; set; } = 0.0m;

        public decimal ValorTotalComDescontoAplicado { get; set; } = 0.0m;

        public bool Ativo { get; set; } = true;

        protected Venda()
        {

        }

        public Venda(Cliente cliente, Guid idCliente, decimal descontoTotal,decimal valorTotalSemDescontoAplicado, decimal valorTotalComDescontoAplicado)
        {
            Cliente = cliente ;
            IdCliente = idCliente;
            DescontoTotal = descontoTotal;
            ValorTotalSemDescontoAplicado = valorTotalSemDescontoAplicado;
            ValorTotalComDescontoAplicado = valorTotalComDescontoAplicado;
        }



        //public Venda(Cliente cliente, decimal desconto, decimal valorTotal)
        //{
        //    Cliente = cliente;
        //    Desconto = desconto;
        //    ValorTotal = valorTotal;
        //}
    }
}
