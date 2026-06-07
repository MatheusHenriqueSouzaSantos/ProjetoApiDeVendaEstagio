using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ServicoDtos
{
    public class ServicoLogDto : BaseDtoLog
    {

        public Guid IdServico { get; private set; }

        public ServicoLogDto(Guid idServico,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdServico = idServico;
        }

    }
}
