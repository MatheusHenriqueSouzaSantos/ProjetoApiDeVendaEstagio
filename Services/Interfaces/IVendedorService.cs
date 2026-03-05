using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IVendedorService
    {
        List<Vendedor> BuscarTodosOsVendedores();

        Vendedor BuscarVendedorPorId(Guid id);

        Vendedor BuscarVendedorPorCpf(string cpf);

        List<Vendedor> BuscarVendedoresPorNome(string nome);

        Vendedor CriarVendedor(VendedorCreateDto dto);

        Vendedor AtualizarVendedor(Guid id,VendedorUpdatedDto dto);

        void DesativarVendedor(Guid id);
    }
}
