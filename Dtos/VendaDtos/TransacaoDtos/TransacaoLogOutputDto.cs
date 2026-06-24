using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos
{
    public class TransacaoLogOutputDto : BaseLogOutputDto
    {
        public Guid IdTransacao { get; set; }
        public TransacaoLogOutputDto(Guid idTransacao,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, 
            DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdTransacao = idTransacao;
        }
    }
}
