using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class EnderecoLogDto : BaseLogDto
    {

        public Guid IdEndereco { get; private set; }
        public Guid IdCliente { get; private set; }

        public EnderecoLogDto(Guid idEndereco,Guid idCliente,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo,
            Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdEndereco = idEndereco;
            IdCliente = idCliente;
        }

    }
}
