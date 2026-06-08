using ApiEstagioBicicletaria.Dtos.ServicoDtos;
using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IServicoService
    {
        List<ServicoDtoOutPut> BuscarServicos();
        ServicoDtoOutPut BuscarServicoPorId(Guid id);

        ServicoDtoOutPut BuscarServicoPorCodigoDoServico(string codigoDoServico);

        Servico CadastrarServico(ServicoInputDto dto);

        Servico AtualizarServico(Guid id, ServicoInputDto dto);

        void DeletarServicoPorId(Guid id);

        List<ServicoDtoOutPut> BuscarServicosPorNome(string nome);

        List<ServicoLogDto> BuscarLogsPorIdServico(Guid id);

    }
}
