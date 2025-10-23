using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            this._clienteService = clienteService;
        }

        [HttpGet]
        public ActionResult<List<Cliente>> BuscarClientes()
        {
            //if (!ModelState.IsValid)
            try
            {
                return _clienteService.BuscarClientes();
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode,ex.Message);
            }
            catch (Exception ex)
            {
                //não retornar a mensagem pois indica exatamente o erro e há o risco de ameaças explorarem
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> BuscarClientePorId([FromRoute] Guid id )
        {
            try
            {
                return _clienteService.BuscarClientePorId(id);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }

        }

        [HttpPost("fisico")]
        public ActionResult<ClienteFisico> CadastrarClienteFisico([FromBody]ClienteFisicoDto dto)
        {
            try
            {
                ClienteFisico clienteFisico= _clienteService.CadastrarClienteFisico(dto);
                return Created($"api/cliente/{clienteFisico.Id}", clienteFisico); 
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }

        }

        [HttpPost("juridico")]
        public ActionResult<ClienteJuridico> CadastrarClienteJuridico([FromBody]ClienteJuridicoDto dto)
        {
            try
            {
                return _clienteService.CadastrarClienteJuridico(dto);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }

        }

        [HttpPut("fisico/{id}")]
        public ActionResult<ClienteFisico> AtualizarClienteFisico([FromRoute] Guid id, [FromBody]ClienteFisicoDto dto)
        {
            try
            {
                return _clienteService.AtualizarClienteFisico(id, dto);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }

        }

        [HttpPut("juridico/{id}")]
        public ActionResult<ClienteJuridico> AtualizarClienteJuridico([FromRoute]Guid id, [FromBody] ClienteJuridicoDto dto)
        {
            try
            {
                return _clienteService.AtualizarClienteJuridico(id, dto);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }

        }

        [HttpDelete("{id}")]
        public ActionResult DeletarClientePorId([FromRoute]Guid id)
        {
            //revisar esssa lógica
            try
            {
                _clienteService.DeletarCLientePorId(id);
                return Ok("Operação realizada com sucesso ");
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }

        }
    }

}
