using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ItemVenda : EntidadeBase
    {
        //todos os atributos deixo que nunca podme ser alterados? se errou crie outro item venda?
        //deixar privado pois ao enviar a venda já esta concluida
        [AnotacaoDeAtributoASerIgnoradoLog]
        public Venda Venda { get; private set; }

        public Guid IdVenda { get; private set; }

        [AnotacaoDeAtributoASerIgnoradoLog]
        public Produto Produto { get; private set; }

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
            Produto = produto;
            Quantidade = quantidade;
            DescontoUnitario = descontoPorUnidade;
            PrecoUnitarioDoProdutoNaVendaSemDesconto = precoUnitarioDoProdutoNaVendaSemDesconto;
        }
    }
}
