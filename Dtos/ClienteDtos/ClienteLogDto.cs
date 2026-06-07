using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteLogDto : BaseDtoLog
    {

        public Guid IdCliente { get; private set; }
        public ClienteLogDto(Guid idCliente,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) 
            : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdCliente = idCliente;
        }


    }
}
