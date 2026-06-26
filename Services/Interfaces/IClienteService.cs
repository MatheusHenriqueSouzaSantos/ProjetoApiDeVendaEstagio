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

        List<ClienteInativoOutputDto> BuscarClientesInativos();
        ClienteDtoOutPut BuscarClientePorId(Guid id);

        ClienteFisico CadastrarClienteFisico(ClienteFisicoCreateDto dto);

        ClienteJuridico CadastrarClienteJuridico(ClienteJuridicoCreateDto dto);

        ClienteFisico AtualizarClienteFisico(Guid id, ClienteFisicoUpdateDto dto);

        ClienteJuridico AtualizarClienteJuridico(Guid id, ClienteJuridicoUpdateDto dto);

        void DeletarCLientePorId(Guid id);

        List<ClienteDtoOutPut> BuscarClientesPorNome(string nome);

        ClienteDtoOutPut BuscarClientePorDocumentoIndentificador (DocumentoClienteInputDto dto);

        List<Object> BuscarLogsClientePorIdCliente(Guid idCliente);

        List<Object> BuscarLogsPorDocumentoIdentificador(DocumentoClienteInputDto dto);

    }
}
