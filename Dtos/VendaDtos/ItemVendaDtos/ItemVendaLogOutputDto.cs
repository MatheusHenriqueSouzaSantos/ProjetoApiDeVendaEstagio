using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos
{
    public class ItemVendaLogOutputDto : BaseLogOutputDto
    {
        public Guid IdItemVenda {  get; set; }

        public Guid IdProdutoDoItem { get; set; }
        public string NomeProdutoDoItem { get; private set; }
        public ItemVendaLogOutputDto(Guid idItemVenda,Guid idProdutoDoItem, string nomeProdutoDoItem,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, 
            Guid idUsuarioResponsavel, DateTime dataCriacao) : base(TipoDtoLog.ItemVenda,acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdItemVenda = idItemVenda;
            IdProdutoDoItem = idProdutoDoItem;
            NomeProdutoDoItem = nomeProdutoDoItem;
        }
    }
}
