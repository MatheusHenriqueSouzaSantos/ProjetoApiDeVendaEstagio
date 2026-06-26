using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class ItemEntradaEstoque : EntidadeBase
    {
        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public EntradaEstoque EntradaEstoque{ get; set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid IdEntradaEstoque { get; set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Produto Produto { get; set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid IdProduto { get; set; }

        public int Quantidade { get; set; }

        public ItemEntradaEstoque(EntradaEstoque entradaEstoque, Produto produto, int quantidade)
        {
            EntradaEstoque = entradaEstoque;
            IdEntradaEstoque = entradaEstoque.Id;
            Produto = produto;
            IdProduto= produto.Id;
            Quantidade = quantidade;
        }
        protected ItemEntradaEstoque() { }

        public ItemEntradaEstoque Copia()
        {
            return new ItemEntradaEstoque(EntradaEstoque, Produto,Quantidade);
        }
    }
}