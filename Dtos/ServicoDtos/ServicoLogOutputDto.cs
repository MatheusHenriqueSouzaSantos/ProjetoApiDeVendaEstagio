using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ServicoDtos
{
    public class ServicoLogOutputDto : BaseLogOutputDto
    {
        public Guid IdServico { get; private set; }

        public ServicoLogOutputDto(Guid idServico,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdServico = idServico;
        }

    }
}
