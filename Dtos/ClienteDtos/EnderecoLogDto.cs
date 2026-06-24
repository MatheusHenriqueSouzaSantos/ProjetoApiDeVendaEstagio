using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class EnderecoLogDto : BaseLogOutputDto
    {

        public Guid IdEndereco { get; private set; }

        public EnderecoLogDto(Guid idEndereco,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo,
            Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdEndereco = idEndereco;
        }

    }
}
