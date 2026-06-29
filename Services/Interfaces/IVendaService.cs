using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IVendaService
    {
        List<VendaTransacaoOutputDto> BuscarTodasVendas();

        List<VendaTransacaoOutputDto> BuscarTodasVendasInativas();

        VendaTransacaoOutputDto BuscarVendaAtivasOuInativasPorId(Guid id);

        VendaTransacaoOutputDto CadastrarVenda(VendaTransacaoCreateDto dto);

        VendaTransacaoOutputDto AtualizarVenda(Guid idVendaEnviado, VendaTransacaoUpdateDto dto);

        void DeletarVendaPorId(Guid idVenda);

        TransacaoOutputDto AtualizarQuantidadeDeParcelasPagasEmUmaTransacao(Guid idTransacaoEnviado, int quantidadeDeParcelasASerAtualizadaParaPaga);

        byte[] GerarRelatorioDeVendasPorPeriodo(DatasParaGeracaoDeRelatorioDto dto);

        List<VendaTransacaoOutputDto> BuscarVendasPorCpfOuCnpj(DocumentoClienteInputDto dto);

        VendaTransacaoOutputDto BuscarVendaAtivaOuInativaPorCodigoVenda(string codigoVenda);

        List<Object> BuscarLogsPorIdVenda(Guid idVenda);

        List<Object> BuscarLogsPorCodigoVenda(string codigoVenda);
    }

}