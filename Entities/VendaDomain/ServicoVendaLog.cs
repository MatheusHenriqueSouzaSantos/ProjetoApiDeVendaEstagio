using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class ServicoVendaLog : EntidadeBaseLog
    {
        public ServicoVenda ServicoVenda { get; private set; }

        public Guid IdServicoVenda { get; private set; }

        public Venda Venda { get; private set; }

        public Guid IdVenda { get; private set; }
        protected ServicoVendaLog()
        {
        }

        public ServicoVendaLog(ServicoVenda servicoVenda, Venda venda,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            ServicoVenda = servicoVenda;
            IdServicoVenda = servicoVenda.Id;
            Venda = venda;
            IdVenda=venda.Id;
        }



    }
}
