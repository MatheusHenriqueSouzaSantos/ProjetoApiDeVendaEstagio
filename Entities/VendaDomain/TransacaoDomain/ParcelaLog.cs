using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.VendaDomain.TransacaoDomain
{
    public class ParcelaLog : EntidadeBaseLog
    {
        public Parcela Parcela { get; private set; }

        public Guid IdParcela { get; private set; }

        public Transacao Transacao { get; private set; }

        public Guid IdTransacao { get; private set; }
        protected ParcelaLog()
        {
        }

        public ParcelaLog(Parcela parcela,Transacao transacao,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Parcela=parcela;
            IdParcela = parcela.Id;
            Transacao = transacao;
            IdTransacao = transacao.Id;
        }



    }
}
