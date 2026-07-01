using ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
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
        public ActionResult<List<EntradaEstoqueOutputDto>> BuscarEntradasAtivas()
        {
            try
            {
                return Ok(_service.BuscarEntradasAtivas());
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<EntradaEstoqueOutputDto>> BuscarEntradasInativas()
        {
            try
            {
                return Ok(_service.BuscarEntradasInativas());
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<EntradaEstoqueOutputDto> BuscarEntradasAtivasPorId([FromRoute]Guid id)
        {
            try
            {
                return Ok(_service.BuscarEntradasAtivasPorId(id));
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
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
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Atualizar([FromRoute] Guid id,EntradaEstoqueUpdateDto dto)
        {
            try
            {
                return Ok(_service.Atualizar(id,dto));
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult InativarEntradaEstoque([FromRoute] Guid id)
        {
            try
            {
                _service.InativarEntradaEstoque(id);
                return Ok("Operação Realizada Com Sucesso");
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("log/{idEntradaEstoque}")]
        public ActionResult<List<Object>> BuscarLogsPorIdEntradaEstoque(Guid idEntradaEstoque)
        {
            try
            {
                return Ok(_service.BuscarLogsPorIdEntradaEstoque(idEntradaEstoque));
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("log/codigo-entrada/{codigoEntrada}")]
        public ActionResult<List<Object>> BuscarLogsPorCodigoEntrada(string codigoEntrada)
        {
            try
            {
                return Ok(_service.BuscarLogsPorCodigoEntrada(codigoEntrada));
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");

            }
        }

    }
}
