using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.EstoqueDtos
{
    public class EstoqueLogOutPutDto : BaseLogOutputDto
    {
        public Guid IdEstoque { get; private set; }
        public Guid IdProdutoDoEstoque { get; private set; }

        public string NomeDoProdutoDoEstoque { get; private set; }

        public EstoqueLogOutPutDto(Guid idEstoque,Guid idProdutoDoEstoque,string nomeDoProdutoDoEstoque, LogAcao acao, string campoAlterado, string valorAntigo
            , string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao)
            : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdEstoque = idEstoque;
            IdProdutoDoEstoque = idProdutoDoEstoque;
            NomeDoProdutoDoEstoque=nomeDoProdutoDoEstoque;
        }
    }
}
