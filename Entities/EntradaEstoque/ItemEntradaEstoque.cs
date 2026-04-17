namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class ItemEntradaEstoque : EntidadeBase
    {

        public EntradaEstoque EntradaEstoque{ get; set; }

        public Guid IdEntradaEstoque { get; set; }

        public Estoque Estoque { get; set; }

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
    }
}