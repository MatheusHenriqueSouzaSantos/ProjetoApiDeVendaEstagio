using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ProdutoDtos
{
    public class ProdutoLogOutputDto : BaseLogOutputDto
    {
        public Guid IdProduto { get; private set; }

        public ProdutoLogOutputDto(Guid idProduto,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdProduto = idProduto;
        }
        
    }
}
