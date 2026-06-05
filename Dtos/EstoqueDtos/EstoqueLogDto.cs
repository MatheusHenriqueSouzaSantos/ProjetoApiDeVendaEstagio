using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.EstoqueDtos
{
    public class EstoqueLogDto : BaseLogDto
    {
        public Guid IdEstoque { get; private set; }

        public EstoqueLogDto(Guid idEstoque, LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo
            , Guid idUsuarioResponsavel, DateTime dataCriacao)
            : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdEstoque = idEstoque;
        }
    }
}
