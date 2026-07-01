using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IClienteService
    {
        List<ClienteDtoOutPut> BuscarClientesAtivos();
        //ActionResult<Cliente> BuscarClientePorId(Guid id);

        List<Object> BuscarClientesInativos();
        ClienteDtoOutPut BuscarClienteAtivoPorId(Guid id);

        ClienteFisico CadastrarClienteFisico(ClienteFisicoCreateDto dto);

        ClienteJuridico CadastrarClienteJuridico(ClienteJuridicoCreateDto dto);

        ClienteFisico AtualizarClienteFisico(Guid id, ClienteFisicoUpdateDto dto);

        ClienteJuridico AtualizarClienteJuridico(Guid id, ClienteJuridicoUpdateDto dto);

        void InativarClientePorId(Guid id);

        void ReativarClientePorId(Guid id);

        List<ClienteDtoOutPut> BuscarClientesPorNome(string nome);

        ClienteDtoOutPut BuscarClientePorDocumentoIndentificador (DocumentoClienteInputDto dto);

        List<Object> BuscarLogsClientePorIdCliente(Guid idCliente);

        List<Object> BuscarLogsPorDocumentoIdentificador(DocumentoClienteInputDto dto);

    }
}
