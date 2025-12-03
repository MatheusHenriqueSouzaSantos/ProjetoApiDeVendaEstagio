using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IClienteService
    {
        List<ClienteDtoOutPut> BuscarClientes();
        //ActionResult<Cliente> BuscarClientePorId(Guid id);
        ClienteDtoOutPut BuscarClientePorId(Guid id);

        ClienteFisico CadastrarClienteFisico(ClienteFisicoDto dto);

        ClienteJuridico CadastrarClienteJuridico(ClienteJuridicoDto dto);

        ClienteFisico AtualizarClienteFisico(Guid id, ClienteFisicoDto dto);

        ClienteJuridico AtualizarClienteJuridico(Guid id, ClienteJuridicoDto dto);

        void DeletarCLientePorId(Guid id);

        List<ClienteDtoOutPut> BuscarClientesPorNome(string nome);

        ClienteDtoOutPut BuscarClientePorDocumentoIndentificador (DocumentoClienteInputDto dto);
    }
}
