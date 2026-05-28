using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.VendedorDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IVendedorService
    {
        List<Vendedor> BuscarTodosOsVendedores();

        Vendedor BuscarVendedorPorId(Guid id);

        Vendedor BuscarVendedorPorCpf(string cpf);

        List<Vendedor> BuscarVendedoresPorNome(string nome);

        Vendedor CadastrarVendedor(VendedorCreateDto dto);

        Vendedor AtualizarVendedor(Guid id,VendedorUpdatedDto dto);

        void DesativarVendedor(Guid id);

        byte[] GerarRelatorioDeVendedoresComMaiorFaturamentoPorPeriodo
            (DatasParaGeracaoDeRelatorioDto dto);

    }
}
