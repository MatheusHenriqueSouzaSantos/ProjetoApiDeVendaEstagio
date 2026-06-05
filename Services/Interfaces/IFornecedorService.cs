using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.FornedorDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IFornecedorService
    {
        List<Fornecedor> BuscarTodos();

        Fornecedor BuscarPorId(Guid id);

        Fornecedor BuscarPorCnpj(string cnpj);

        List<Fornecedor> BuscarPorNome(string nome);

        Fornecedor Cadastrar(FornecedorCreateDto dto);

        Fornecedor Atualizar(Guid id,FornecedorUpdateDto dto);

        void Desativar(Guid id);

        byte[] GerarRelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo(DatasParaGeracaoDeRelatorioDto dto);

        List<FornecedorLogDto> BuscarLogsPorIdFornecedor(Guid id);

    }
}
