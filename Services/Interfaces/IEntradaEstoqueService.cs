using ApiEstagioBicicletaria.Entities.EntradaEstoque;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IEntradaEstoqueService
    {
        List<EntradaEstoqueOutputDto> BuscarTodos();

        EntradaEstoqueOutputDto BuscarPorId(Guid id);

        EntradaEstoqueOutputDto Cadastrar(EntradaEstoqueInputDto dto);

        EntradaEstoqueOutputDto Atualizar(Guid id, EntradaEstoqueInputDto dto);

        void InativarEntradaEstoque(Guid id);
    }
}
