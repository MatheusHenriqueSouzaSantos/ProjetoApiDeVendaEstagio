using ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IEntradaEstoqueService
    {
        List<EntradaEstoqueOutputDto> BuscarTodos();

        List<EntradaEstoqueOutputDto> BuscarTodosInativos();

        EntradaEstoqueOutputDto BuscarPorId(Guid id);

        EntradaEstoqueOutputDto Cadastrar(EntradaEstoqueCreateDto dto);

        EntradaEstoqueOutputDto Atualizar(Guid id, EntradaEstoqueUpdateDto dto);

        void InativarEntradaEstoque(Guid id);

        List<Object> BuscarLogsPorIdEntradaEstoque(Guid idEntrada);

        List<Object> BuscarLogsPorCodigoEntrada(string codigoEntrada);

    }
}
