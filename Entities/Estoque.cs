using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities
{
    public class Estoque : EntidadeBase
    {

        public Produto Produto { get; set; }

        public Guid ProdutoId { get; set; }


        public int QuantidadeEmEstoque { get; set; } = 0;


        public Estoque(Produto produto, Guid produtoId)
        {
            Produto = produto;
            ProdutoId = produtoId;
        }

        protected Estoque()
        {

        }
    }
}
