using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Utils;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class Venda : EntidadeBase
    {
        public string CodigoVenda { get; private set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Cliente Cliente { get;  set; }

        public Guid IdCliente { get;  set; }

        public decimal DescontoTotal { get; set; } = 0.0m;
        //pegar o valor total pela soma de todos os itens menos o desconto

        public decimal ValorTotalSemDesconto { get; set; } = 0.0m;

        public decimal ValorTotalComDesconto { get; set; } = 0.0m;

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Vendedor Vendedor { get; set; }

        public Guid IdVendedor {  get; set; }

        protected Venda()
        {

        }

        public Venda(string codigoVenda, Cliente cliente, decimal descontoTotal, decimal valorTotalSemDesconto, decimal valorTotalComDesconto, Vendedor vendedor)
        {
            CodigoVenda = codigoVenda;
            Cliente = cliente;
            IdCliente = cliente.Id;
            DescontoTotal = descontoTotal;
            ValorTotalSemDesconto = valorTotalSemDesconto;
            ValorTotalComDesconto = valorTotalComDesconto;
            Vendedor = vendedor;
            IdVendedor = vendedor.Id;
        }


        public Venda Copia()
        {
            return new Venda(CodigoVenda, Cliente, DescontoTotal, ValorTotalSemDesconto, ValorTotalComDesconto, Vendedor);
        }



        //excluir esse construtor depois





        //public Venda(Cliente cliente, decimal desconto, decimal valorTotal)
        //{
        //    Cliente = cliente;
        //    Desconto = desconto;
        //    ValorTotal = valorTotal;
        //}
    }
}
