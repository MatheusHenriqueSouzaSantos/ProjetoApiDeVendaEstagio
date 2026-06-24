using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos
{
    public class ItemVendaLogOutputDto : BaseLogOutputDto
    {
        public Guid IdItemVenda {  get; set; }
        public string NomeProdutoRelacionado { get; private set; }
        public ItemVendaLogOutputDto(Guid idItemVenda,string nomeProdutoRelacionado,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, 
            Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdItemVenda = idItemVenda;
            NomeProdutoRelacionado = nomeProdutoRelacionado;
        }
    }
}
