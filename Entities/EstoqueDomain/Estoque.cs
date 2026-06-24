using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities.EstoqueDomain
{
    public class Estoque : EntidadeBase
    {
        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Produto Produto { get; set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid ProdutoId { get; set; }

        public int QuantidadeEmEstoque { get; set; } = 0;


        public Estoque(Produto produto)
        {
            Produto = produto;
            ProdutoId = produto.Id;
        }

        protected Estoque()
        {

        }


        public void AdicionarQuantidadeEmEstoque(int quantidadeASerAdicionar)
        {
            QuantidadeEmEstoque += quantidadeASerAdicionar;
        }
        public void AbaterQuantidadeEmEstoque(int quantidadeASerAbatida)
        {
            QuantidadeEmEstoque-=quantidadeASerAbatida;
        }
    }
}
