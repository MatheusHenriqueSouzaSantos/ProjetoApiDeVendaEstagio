using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.VendedorDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IVendedorService
    {
        List<Vendedor> BuscarTodosOsVendedoresAtivos();

        List<Vendedor> BuscarTodosOsVendedoresInativos();

        Vendedor BuscarVendedorAtivoPorId(Guid id);

        Vendedor BuscarVendedorPorCpf(string cpf);

        List<Vendedor> BuscarVendedoresPorNome(string nome);

        Vendedor CadastrarVendedor(VendedorCreateDto dto);

        Vendedor AtualizarVendedor(Guid id,VendedorUpdatedDto dto);

        void InativarVendedor(Guid id);

        void ReativarVendedor(Guid id);

        byte[] GerarRelatorioDeVendedoresComMaiorFaturamentoPorPeriodo
            (DatasParaGeracaoDeRelatorioDto dto);

        List<VendedorLogOutputDto> BuscarLogsPorIdVendedor(Guid id);

        List<VendedorLogOutputDto> BuscarLogsPorCpf(string cpf);

    }
}
