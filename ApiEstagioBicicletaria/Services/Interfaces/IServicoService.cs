using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IServicoService
    {
        List<Servico> BuscarServicos();
        Servico BuscarServicoPorId(Guid id);

        Servico BuscarServicoPorCodigoServico(string codigoDoServico);

        Servico CadastrarServico(ServicoDto dto);

        Servico AtualizarServico(Guid id, ServicoDto dto);

        void DeletarServicoPorId(Guid id);

    }
}
