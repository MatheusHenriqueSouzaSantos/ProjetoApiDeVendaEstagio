using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
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
        public ActionResult<List<Cliente>> BuscarClientes()
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
        public ActionResult<Cliente> BuscarClientePorId([FromRoute,Required(ErrorMessage ="O id é obrigatório")] Guid id )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
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
        public ActionResult DeletarClientePorId([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id)
        {
            //revisar esssa lógica
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
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
