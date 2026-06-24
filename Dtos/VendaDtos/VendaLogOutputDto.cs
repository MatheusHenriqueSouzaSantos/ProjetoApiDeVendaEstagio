using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaLogOutputDto : BaseLogOutputDto
    {
        Guid IdVenda {  get; set; }
        public VendaLogOutputDto(Guid idVenda,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao)
            : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdVenda = idVenda;
        }
    }
}
