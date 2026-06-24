using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain
{
    public class TransacaoLog : EntidadeBaseLog
    {
        public Transacao Transacao { get; private set; }
        public Guid IdTransacao { get; private set; }

        public Venda Venda { get; private set; }

        public Guid IdVenda { get; private set; }
        protected TransacaoLog()
        {
        }

        public TransacaoLog(Transacao transacao,Venda venda,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {

            Transacao = transacao;
            IdTransacao = transacao.Id;
            Venda = venda;
            IdVenda = venda.Id;

        }
    }
}
