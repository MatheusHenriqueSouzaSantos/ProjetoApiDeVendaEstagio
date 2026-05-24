using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain
{
    public class VendaLog : EntidadeBaseLog
    {

        public Venda Venda { get; private set; }
        public Guid IdVenda { get; private set; }
        protected VendaLog()
        {
        }

        public VendaLog(Venda venda,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Venda = venda;
            IdVenda = venda.Id;
        }


    }
}
