using ApiEstagioBicicletaria.Dtos.EstoqueDtos;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IEstoqueService
    {
        EstoqueSimplificadoOutputDto BuscarPorId(Guid id);

        EstoqueSimplificadoOutputDto AdicionarQuantidadeEmEstoque(Guid id, int quantidade);

        EstoqueSimplificadoOutputDto AbaterQuantidadeEmEstoque(Guid id, int quantidade);

        List<EstoqueLogDto> BuscarLogsPorIdEstoque(Guid id);
    }
}
