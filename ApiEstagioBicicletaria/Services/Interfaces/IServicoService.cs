using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IServicoService
    {
        List<ServicoDtoOutPut> BuscarServicos();
        ServicoDtoOutPut BuscarServicoPorId(Guid id);

        ServicoDtoOutPut BuscarServicoPorCodigoDoServico(string codigoDoServico);

        Servico CadastrarServico(ServicoDto dto);

        Servico AtualizarServico(Guid id, ServicoDto dto);

        void DeletarServicoPorId(Guid id);

        List<ServicoDtoOutPut> BuscarServicosPorNome(string nome);

    }
}
