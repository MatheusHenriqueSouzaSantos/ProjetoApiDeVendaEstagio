using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.VendaDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IVendaService
    {
        Venda CriarVenda(VendaTransacaoDto vendaTransacaoDto);

    }

}