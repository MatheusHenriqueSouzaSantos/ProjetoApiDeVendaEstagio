using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IVendaService
    {
        List<VendaTransacaoOutputDto> BuscarTodasVendas();

        VendaTransacaoOutputDto BuscarVendaPorId(Guid id);

        VendaTransacaoOutputDto CadastrarVenda(VendaTransacaoInputDto dto);

        VendaTransacaoOutputDto AtualizarVenda(Guid idVendaEnviado, VendaTransacaoInputDto dto);

        void DeletarVendaPorId(Guid idVenda);

        TransacaoOutputDto AtualizarQuantidadeDeParcelasPagasEmUmaTransacao(Guid idTransacaoEnviado, int quantidadeDeParcelasASerAtualizadaParaPaga);

        byte[] GerarRelatorioDeVendasPorPeriodo(DatasParaGeracaoDeRelatorioDto dto);

        List<VendaTransacaoOutputDto> BuscarVendasPorCpfOuCnpj(DocumentoClienteInputDto dto);
    }

}