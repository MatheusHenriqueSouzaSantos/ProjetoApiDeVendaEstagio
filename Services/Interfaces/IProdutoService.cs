using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IProdutoService
    {
        List<ProdutoDtoOutPut> BuscarProdutosAtivos();

        List<ProdutoInativoOutputDto> BuscarProdutosInativos();
        ProdutoDtoOutPut BuscarProdutoAtivoPorId(Guid id);

        ProdutoDtoOutPut BuscarProdutoAtivoPorCodigoDeBarra(string codigoDeBarra);

        Produto CadastrarProduto(ProdutoInputDto dto);

        Produto AtualizarProduto(Guid id, ProdutoInputDto dto);

        void InativarProdutoPorId(Guid id);

        void ReativarProdutoPorId(Guid id);

        List<ProdutoDtoOutPut> BuscarProdutosPorNome(string nome);
        byte[] GerarRelatorioDeProdutosMaisVendidosPorPeriodo(DatasParaGeracaoDeRelatorioDto dto);
        byte[] GerarRelatorioDeProdutosComEstoqueAbaixoOuIgualUmaQuantidade(int quantidadePaBuscarProdutosEmFalta);

        List<Object> BuscarLogsPorIdProduto(Guid idProdutoEnviado);

        List<Object> BuscarLogsPorCodigoDeBarra(string codigoDeBarra);

        //List<BaseDtoLog> BuscarLogsGeraisPorPaginacao(int paginaASerBuscada,int quantidadeDeRegistrosPorPagina);
    }
}
