using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IProdutoService
    {
        List<ProdutoDtoOutPut> BuscarProdutos();
        ProdutoDtoOutPut BuscarProdutoPorId(Guid id);

        ProdutoDtoOutPut BuscarProdutoPorCodigoDeBarra(string codigoDeBarra);

        Produto CadastrarProduto(ProdutoInputDto dto);

        Produto AtualizarProduto(Guid id, ProdutoInputDto dto);

        void DeletarProdutoPorId(Guid id);

        List<ProdutoDtoOutPut> BuscarProdutosPorNome(string nome);
        byte[] GerarRelatorioDeProdutosComMaiorFaturamentoPorPeriodo(DatasParaGeracaoDeRelatorioDto dto);
        byte[] GerarRelatorioDeProdutosComEstoqueAbaixoOuIgualUmaQuantidade(int quantidadePaBuscarProdutosEmFalta);
    }
}
