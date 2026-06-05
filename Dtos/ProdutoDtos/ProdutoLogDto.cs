using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ProdutoDtos
{
    public class ProdutoLogDto : BaseLogDto
    {

        public Guid IdProduto { get; private set; }

        public ProdutoLogDto(Guid idProduto,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdProduto = idProduto;
        }
        
    }
}
