using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Output
{
    public class ItemEntradaEstoqueLogOutputDto : BaseLogOutputDto
    {
        public Guid IdItemEntradaEstoque {  get; set; }

        public Guid IdProdutoDoItem { get; set; }

        public string NomeDoProdutoDoItem { get; set; } 
        public ItemEntradaEstoqueLogOutputDto(Guid idItemEntradaEstoque,Guid idProdutoDoItem,string nomeProdutoDoItem,
            LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo,Guid idUsuarioResponsavel, DateTime dataCriacao) 
            : base(TipoDtoLog.ItemEntradaEstoque, acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdItemEntradaEstoque = idItemEntradaEstoque;
            IdProdutoDoItem = idProdutoDoItem;
            NomeDoProdutoDoItem=nomeProdutoDoItem;
        }
    }
}
