using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos
{
    public class BaseLogDto
    {
        public LogAcao Acao { get; private set; }

        public string CampoAlterado { get; private set; }

        public string ValorAntigo { get; private set; }

        public string ValorNovo { get; private set; }

        public Guid IdUsuarioResponsavel { get; private set; }

        public DateTime DataCriacao { get; private set; } = DateTime.Now;

        public BaseLogDto(LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo
            , Guid idUsuarioResponsavel, DateTime dataCriacao)
        {
            Acao = acao;
            CampoAlterado = campoAlterado;
            ValorAntigo = valorAntigo;
            ValorNovo = valorNovo;
            IdUsuarioResponsavel = idUsuarioResponsavel;
            DataCriacao = dataCriacao;
        }
    }
}
