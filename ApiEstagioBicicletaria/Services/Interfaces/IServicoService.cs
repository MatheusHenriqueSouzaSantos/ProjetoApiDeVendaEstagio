using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities.ServicoDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IServicoService
    {
        List<Servico> BuscarServicos();
        Servico BuscarServicoPorId(Guid id);

        Servico BuscarServicoPorCodigoDoServico(string codigoDoServico);

        Servico CadastrarServico(ServicoDto dto);

        Servico AtualizarServico(Guid id, ServicoDto dto);

        void DeletarServicoPorId(Guid id);

        List<Servico> BuscarServicosPorNome(string nome);

    }
}
