using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/entrada-estoque")]
    public class EntradaEstoqueController : ControllerBase
    {
        private readonly IEntradaEstoqueService _service;

        public EntradaEstoqueController(IEntradaEstoqueService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<EntradaEstoqueOutputDto>> BuscarTodos()
        {
            try
            {
                return Ok(_service.BuscarTodos());
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<EntradaEstoqueOutputDto> BuscarPorId([FromRoute]Guid id)
        {
            try
            {
                return Ok(_service.BuscarPorId(id));
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<EntradaEstoqueOutputDto> Cadastrar([FromBody] EntradaEstoqueCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                EntradaEstoqueOutputDto entradaCadastrada=_service.Cadastrar(dto);
                return Created($"api/{entradaCadastrada.Id}", entradaCadastrada);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult InativarEntradaEstoque([FromRoute] Guid id)
        {
            try
            {
                _service.InativarEntradaEstoque(id);
                return NoContent();
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno");
            }
        }

    }
}
