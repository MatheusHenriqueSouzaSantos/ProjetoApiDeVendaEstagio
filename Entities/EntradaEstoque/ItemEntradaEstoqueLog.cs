using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Entities.EntradaEstoque
{
    public class ItemEntradaEstoqueLog : EntidadeBaseLog
    {

        public ItemEntradaEstoque ItemEntradaEstoque { get; private set; }
        public Guid IdItemEntradaEstoque { get; private set; }

        public Venda Venda { get; private set; }
        public Guid IdVenda { get; private set; }
        public ItemEntradaEstoqueLog()
        {
        }

        public ItemEntradaEstoqueLog(ItemEntradaEstoque itemEntradaEstoque, Venda venda,LogAcao acao, string campoAlterado, string valorAntigo,
            string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            ItemEntradaEstoque = itemEntradaEstoque;
            IdItemEntradaEstoque = itemEntradaEstoque.Id;
            Venda = venda;
            IdVenda = venda.Id; 
        }


    }
}
