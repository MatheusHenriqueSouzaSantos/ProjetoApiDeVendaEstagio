using ApiEstagioBicicletaria.Dtos.ServicoDtos;
using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IServicoService
    {
        List<ServicoDtoOutPut> BuscarServicosAtivos();

        List<ServicoInativoOutputDto> BuscarServicosInativos();
        ServicoDtoOutPut BuscarServicoAtivoPorId(Guid id);

        ServicoDtoOutPut BuscarServicoAtivoPorCodigoDoServico(string codigoDoServico);

        Servico CadastrarServico(ServicoInputDto dto);

        Servico AtualizarServico(Guid id, ServicoInputDto dto);

        void InativarServicoPorId(Guid id);

        void ReativarServicoPorId(Guid id);

        List<ServicoDtoOutPut> BuscarServicosPorNome(string nome);

        List<ServicoLogOutputDto> BuscarLogsPorIdServico(Guid id);

        List<ServicoLogOutputDto> BuscarLogsPorCodigoDoServico(string codigoDoServico);

    }
}
