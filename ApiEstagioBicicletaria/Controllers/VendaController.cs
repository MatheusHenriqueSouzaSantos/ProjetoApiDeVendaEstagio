using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class VendaController : ControllerBase
    {
        private IVendaService _vendaService;

        public VendaController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet]
        public ActionResult<List<VendaTransacaoOutputDto>> BuscarTodasVendas()
        {
            try
            {
                return Ok(_vendaService.BuscarTodasVendas());
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

        [HttpGet("{id}")]
        public ActionResult<VendaTransacaoOutputDto> BuscarVendaPorId([FromRoute]string id)
        {
            try
            {
                Guid idConvertido;
                if (!Guid.TryParse(id, out idConvertido))
                {
                    return BadRequest("id no formato inválido de GUID");
                }
                return Ok(_vendaService.BuscarVendaPorId(idConvertido));
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

        [HttpPost]
        public ActionResult<VendaTransacaoOutputDto> CadastrarVenda([FromBody] VendaTransacaoInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return Ok(_vendaService.CadastrarVenda(dto));
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

        [HttpPut("{id}")]
        public ActionResult<VendaTransacaoOutputDto> AtualizarVenda([FromRoute]string id, [FromBody] VendaTransacaoInputDto dto)
        {
            try
            {
                Guid idConvertido;
                if (!Guid.TryParse(id, out idConvertido))
                {
                    return BadRequest("id no formato inválido de GUID");
                }
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return Ok(_vendaService.AtualizarVenda(idConvertido, dto));
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

        [HttpDelete("{id}")]
        public ActionResult DeletarVendaPorId([FromRoute] string id)
        {
            try
            {
                Guid idConvertido;
                if (!Guid.TryParse(id, out idConvertido))
                {
                    return BadRequest("id no formato inválido de GUID");
                }
                _vendaService.DeletarVendaPorId(idConvertido);
                return Ok("Operação Realizada com Sucesso !!");
            }
            catch(ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Erro Inesperado");
            }
        }

    }
}
