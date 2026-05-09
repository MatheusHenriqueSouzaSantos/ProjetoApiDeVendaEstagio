using ApiEstagioBicicletaria.Entities.EntradaEstoque;

namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class EntradaEsoqueComSeusItensDto
    {
        public EntradaEstoque EntradaEstoque { get; private set; }

        public List<ItemEntradaEstoque> Itens { get; private set; }

        public EntradaEsoqueComSeusItensDto(EntradaEstoque entradaEstoque, List<ItemEntradaEstoque> itens)
        {
            EntradaEstoque = entradaEstoque;
            Itens = itens;
        }
    }
}
