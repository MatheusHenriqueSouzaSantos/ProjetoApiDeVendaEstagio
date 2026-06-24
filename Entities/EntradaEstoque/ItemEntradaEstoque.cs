using ApiEstagioBicicletaria.Entities.EstoqueDomain;

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
        public Estoque Estoque { get; set; }

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid IdEstoque { get; set; }

        public int Quantidade { get; set; }

        public ItemEntradaEstoque(EntradaEstoque entradaEstoque, Estoque estoque, int quantidade)
        {
            EntradaEstoque = entradaEstoque;
            IdEntradaEstoque = entradaEstoque.Id;
            Estoque = estoque;
            IdEstoque= estoque.Id;
            Quantidade = quantidade;
        }
        protected ItemEntradaEstoque() { }
    }
}