using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IProdutoService
    {
        List<Produto> BuscarProdutos();
        Produto BuscarProdutoPorId(Guid id);

        Produto BuscarProdutoPorCodigoDeBarra(string codigoDeBarra);

        Produto CadastrarProduto(ProdutoDto dto);

        Produto AtualizarProduto(Guid id, ProdutoDto dto);

        void DeletarProdutoPorId(Guid id);

        void DefinirQuantidadeEmEstoqueDeProduto(Guid id, int quantidade);
    }
}
