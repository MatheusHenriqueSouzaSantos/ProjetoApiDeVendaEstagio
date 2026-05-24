using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Entities
{
    public abstract class EntidadeBaseLog
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public LogAcao Acao { get; private set; }

        public string CampoAlterado { get; private set; }
        public string ValorAntigo { get; private set; }

        public string ValorNovo { get; private set; }

        public Usuario UsuarioResponsavel { get; private set; }

        public Guid IdUsuarioResponsavel { get; private set; }

        public DateTime DataCriacao { get; private set; }=DateTime.Now;

        protected EntidadeBaseLog(LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Usuario usuarioResponsavel)
        {
            Acao = acao;
            CampoAlterado = campoAlterado;
            ValorAntigo = valorAntigo;
            ValorNovo = valorNovo;
            UsuarioResponsavel = usuarioResponsavel;
            IdUsuarioResponsavel=usuarioResponsavel.Id
        }

        protected EntidadeBaseLog()
        {
        }
    }
}
