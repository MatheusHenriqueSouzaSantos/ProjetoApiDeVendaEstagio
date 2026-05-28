using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class ItemEntradaEstoqueLog : EntidadeBaseLog
    {

        public ItemEntradaEstoque ItemEntradaEstoque { get; private set; }
        public Guid IdItemEntradaEstoque { get; private set; }

        public EntradaEstoque EntradaEstoque { get; private set; }
        public Guid IdEntradaEstoque { get; private set; }
        public ItemEntradaEstoqueLog()
        {
        }

        public ItemEntradaEstoqueLog(ItemEntradaEstoque itemEntradaEstoque, EntradaEstoque entradaEstoque,LogAcao acao, string campoAlterado, string valorAntigo,
            string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            ItemEntradaEstoque = itemEntradaEstoque;
            IdItemEntradaEstoque = itemEntradaEstoque.Id;
            EntradaEstoque = entradaEstoque;
            IdEntradaEstoque = entradaEstoque.Id; 
        }


    }
}
