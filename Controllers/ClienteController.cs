using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        [Authorize]
        public ActionResult<List<ClienteDtoOutPut>> BuscarClientes()
        {
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
        [Authorize]
        public ActionResult<ClienteDtoOutPut> BuscarClientePorId([FromRoute] Guid id )
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
        [Authorize]
        public ActionResult<ClienteFisico> CadastrarClienteFisico([FromBody]ClienteFisicoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }

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
        [Authorize]
        public ActionResult<ClienteJuridico> CadastrarClienteJuridico([FromBody]ClienteJuridicoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                ClienteJuridico clienteJuridico = _clienteService.CadastrarClienteJuridico(dto);
                return Created($"api/cliente/{clienteJuridico.Id}", clienteJuridico);
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
        [Authorize]
        public ActionResult<ClienteFisico> AtualizarClienteFisico([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id, [FromBody]ClienteFisicoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }

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
        [Authorize]
        public ActionResult<ClienteJuridico> AtualizarClienteJuridico([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id, [FromBody] ClienteJuridicoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
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
        [Authorize]
        public ActionResult DeletarClientePorId([FromRoute] Guid id)
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

        [HttpGet("buscar-clientes-por-nome/{nome}")]
        [Authorize]
        //pedir para mandar se quer que venha fisicos ou júridicos
        public ActionResult<List<ClienteDtoOutPut>> BuscarClientesPorNome([FromRoute] string nome)
        {
            try
            {
                return _clienteService.BuscarClientesPorNome(nome);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado");
            }
        }

        [HttpPost("buscar-cliente-por-documento-indentificador")]
        [Authorize]
        public ActionResult<ClienteDtoOutPut> BuscarClientePorDocumentoIndentificador([FromBody] DocumentoClienteInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return _clienteService.BuscarClientePorDocumentoIndentificador(dto);
            }
            catch(ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,"Erro Inesperado");
            }
        }

        [Authorize]
        [HttpGet("log/{idCliente}")]
        public ActionResult<List<BaseDtoLog>> BuscarLogsPorIdCliente(Guid idCliente)
        {
            try
            {
                return Ok(_clienteService.BuscarLogsClientePorIdCliente(idCliente));
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado");
                //return StatusCode(500, ex.Message);

            }
        }

    }

}
