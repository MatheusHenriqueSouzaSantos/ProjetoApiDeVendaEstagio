using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ItemVenda : EntidadeBase
    {

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Venda Venda { get; private set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid IdVenda { get; private set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Produto Produto { get; private set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid IdProduto { get; private set; }

        public int Quantidade { get; set; }

        public decimal DescontoUnitario { get;  set; }

        public decimal PrecoUnitarioDoProdutoNaVendaSemDesconto { get;  set; }


        protected ItemVenda()
        {

        }

        public ItemVenda(Venda venda, Produto produto, int quantidade, decimal descontoPorUnidade, decimal precoUnitarioDoProdutoNaVendaSemDesconto)
        {
            Venda = venda;
            IdVenda = venda.Id;
            Produto = produto;
            IdProduto=produto.Id;
            Quantidade = quantidade;
            DescontoUnitario = descontoPorUnidade;
            PrecoUnitarioDoProdutoNaVendaSemDesconto = precoUnitarioDoProdutoNaVendaSemDesconto;
        }

        public ItemVenda Copia()
        {
            return new ItemVenda(Venda, Produto, Quantidade, DescontoUnitario, PrecoUnitarioDoProdutoNaVendaSemDesconto);
        }
    }
}
