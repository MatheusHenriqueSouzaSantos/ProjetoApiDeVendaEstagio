using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IProdutoService
    {
        List<ProdutoDtoOutPut> BuscarProdutos();

        List<ProdutoInativoOutputDto> BuscarProdutosInativos();
        ProdutoDtoOutPut BuscarProdutoPorId(Guid id);

        ProdutoDtoOutPut BuscarProdutoPorCodigoDeBarra(string codigoDeBarra);

        Produto CadastrarProduto(ProdutoInputDto dto);

        Produto AtualizarProduto(Guid id, ProdutoInputDto dto);

        void DeletarProdutoPorId(Guid id);

        List<ProdutoDtoOutPut> BuscarProdutosPorNome(string nome);
        byte[] GerarRelatorioDeProdutosComMaiorQuantidadeVendidaPorPeriodo(DatasParaGeracaoDeRelatorioDto dto);
        byte[] GerarRelatorioDeProdutosComEstoqueAbaixoOuIgualUmaQuantidade(int quantidadePaBuscarProdutosEmFalta);

        List<Object> BuscarLogsPorIdProduto(Guid idProdutoEnviado);

        List<Object> BuscarLogsPorCodigoDeBarra(string codigoDeBarra);

        //List<BaseDtoLog> BuscarLogsGeraisPorPaginacao(int paginaASerBuscada,int quantidadeDeRegistrosPorPagina);
    }
}
