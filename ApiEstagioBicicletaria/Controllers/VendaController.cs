using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPatch("{idTransacao}")]
        //so retorno a transação ou a venda também??,  receber o id da transação ou da venda???
        //fica dificl pro front enviar o id da transação???
        public ActionResult<TransacaoOutputDto> AtualizarQuantidadeDeParcelasPagasEmUmTransacao([FromRoute]string idTransacao, [FromBody]AtualizarQuantidadeDeParcelasPagasInputDto dto)
        {
            try
            {
                Guid idTransacaoConvertido;
                if (!Guid.TryParse(idTransacao, out idTransacaoConvertido))
                {
                    return BadRequest("id no formato inválido de GUID");
                }
                TransacaoOutputDto transacaoASerRetornada= _vendaService.AtualizarQuantidadeDeParcelasPagasEmUmaTransacao(idTransacaoConvertido,dto.QuantidadeDeParcelasASerAtualizadaParaPaga);
                return Ok(transacaoASerRetornada);
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

        [HttpPost("relatorio-de-vendas-por-periodo")]
        public ActionResult<byte[]> GerarRelatoriosDeVendaPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
        {
            try
            {
                byte[] bytesPdf= _vendaService.GerarRelatorioDeVendasPorPeriodo(dto);
                return File(bytesPdf, "application/pdf", "relatorioDeVendasPorPeriodo.pdf");
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
        [HttpGet("buscar-vendas-por-documento-indentificador-do-cliente")]
        public ActionResult<List<VendaTransacaoOutputDto>> BuscarVendasPorCpfOuCnpj(DocumentoClienteInputDto dto)
        {
            try
            {
                return _vendaService.BuscarVendasPorCpfOuCnpj(dto);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Erro no Servidor");
            }
        }
    }
}
