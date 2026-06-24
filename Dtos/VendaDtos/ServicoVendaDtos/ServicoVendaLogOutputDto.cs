using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos
{
    public class ServicoVendaLogOutputDto : BaseLogOutputDto
    {
        public Guid IdServicoVenda { get; set; }
        public string NomeServicoRelacionado { get; private set; }
        public ServicoVendaLogOutputDto(Guid idServicoVenda, string nomeServicoRelacionado, LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo,
            Guid idUsuarioResponsavel, DateTime dataCriacao) : base(acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao)
        {
            IdServicoVenda = idServicoVenda;
            NomeServicoRelacionado = nomeServicoRelacionado;
        }
    }
}
