using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IVendaService
    {
        List<VendaTransacaoOutputDto> BuscarTodasVendas();

        VendaTransacaoOutputDto BuscarVendaPorId(Guid id);

        VendaTransacaoOutputDto CadastrarVenda(VendaTransacaoInputDto dto);

        VendaTransacaoOutputDto AtualizarVenda(Guid id, VendaTransacaoInputDto dto);

        void DeletarVendaPorId(Guid id);

        TransacaoOutputDto AtualizarQuantidadeDeParcelasPagasEmUmaTransacao(Guid idTransacao,int quantidadeDeParcelasPagas);


    }

}