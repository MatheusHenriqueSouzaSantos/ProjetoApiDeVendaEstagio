using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IFornecedorService
    {
        List<Fornecedor> BuscarTodos();

        Fornecedor BuscarPorId(Guid id);

        Fornecedor BuscarPorCnpj(string cnpj);

        Fornecedor Cadastrar(FornecedorCreateDto dto);

        Fornecedor Atualizar(Guid id,FornecedorUpdateDto dto);

        void Desativar(Guid id);

        List<Fornecedor> BuscarFornecedoresPorNome(string nome);
    }
}
