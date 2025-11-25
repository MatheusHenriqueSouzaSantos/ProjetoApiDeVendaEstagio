using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IClienteService
    {
        List<Cliente> BuscarClientes();
        //ActionResult<Cliente> BuscarClientePorId(Guid id);
        Cliente BuscarClientePorId(Guid id);

        ClienteFisico CadastrarClienteFisico(ClienteFisicoDto dto);

        ClienteJuridico CadastrarClienteJuridico(ClienteJuridicoDto dto);

        ClienteFisico AtualizarClienteFisico(Guid id, ClienteFisicoDto dto);

        ClienteJuridico AtualizarClienteJuridico(Guid id, ClienteJuridicoDto dto);

        void DeletarCLientePorId(Guid id);

        List<Cliente> BuscarClientesPorNome(string nome);

        Cliente BuscarClientePorDocumentoIndentificador (ClienteDocumentoInputDto dto);
    }
}
