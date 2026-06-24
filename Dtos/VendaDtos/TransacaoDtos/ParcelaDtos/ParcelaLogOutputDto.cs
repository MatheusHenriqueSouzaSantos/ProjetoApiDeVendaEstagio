using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos.ParcelaDtos
{
    public class ParcelaLogOutputDto : BaseLogOutputDto
    {
        public Guid IdParcela { get; set; }

        public int NumeroDaParcelaDaVenda { get; set; }
        public ParcelaLogOutputDto(Guid idParcela,int numeroDaParcelaDaVenda, LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdParcela = idParcela;
            NumeroDaParcelaDaVenda=numeroDaParcelaDaVenda;
        }
    }
}
