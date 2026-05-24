using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities.ServicoDomain
{
    public class ServicoLog : EntidadeBaseLog
    {
        public Servico Servico { get; private set; }
        public Guid IdServico { get; private set; }
        public ServicoLog()
        {
        }

        public ServicoLog(Servico servico,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, usuarioResponsavel)
        {
            Servico = servico;
            IdServico = servico.Id;
        }

    }
}
