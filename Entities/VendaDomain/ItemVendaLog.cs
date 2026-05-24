using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ItemVendaLog : EntidadeBaseLog
    {

        public ItemVenda ItemVenda { get; private set; }

        public Guid IdItemVenda {  get; private set; }

        public Venda Venda { get; private set; }

        public Guid IdVenda { get; private set; }
        protected ItemVendaLog()
        {
        }

        public ItemVendaLog(ItemVenda itemVenda, Venda venda, LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            ItemVenda = itemVenda;
            IdItemVenda = itemVenda.Id;
            Venda = venda;
            IdVenda = venda.Id;
        }
    }
}
