using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.FornedorDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IFornecedorService
    {
        List<Fornecedor> BuscarTodosAtivos();

        List<Fornecedor> BuscarTodosInativos();

        Fornecedor BuscarAtivoPorId(Guid id);

        Fornecedor BuscarAtivoPorCnpj(string cnpj);

        List<Fornecedor> BuscarAtivoPorNome(string nome);

        Fornecedor Cadastrar(FornecedorCreateDto dto);

        Fornecedor Atualizar(Guid id,FornecedorUpdateDto dto);

        void Inativar(Guid id);

        void Reativar(Guid id);

        byte[] GerarRelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo(DatasParaGeracaoDeRelatorioDto dto);

        List<FornecedorLogOutputDto> BuscarLogsPorIdFornecedor(Guid id);

        List<FornecedorLogOutputDto> BuscarLogsPorCnpj(string cnpj);

    }
}
