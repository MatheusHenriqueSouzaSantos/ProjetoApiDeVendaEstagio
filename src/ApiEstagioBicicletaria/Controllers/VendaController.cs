using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.TransacaoDtos.ParcelaDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize]
        public ActionResult<List<VendaTransacaoOutputDto>> BuscarVendasAtivas()
        {
            try
            {
                return Ok(_vendaService.BuscarVendasAtivas());
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

        [HttpGet("inativos")]
        [Authorize]
        public ActionResult<List<VendaTransacaoOutputDto>> BuscarVendasInativas()
        {
            try
            {
                return Ok(_vendaService.BuscarVendasInativas());
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
        public ActionResult<VendaTransacaoOutputDto> BuscarVendaAtivaPorId([FromRoute]Guid id)
        {
            try
            {
                return Ok(_vendaService.BuscarVendaAtivasPorId(id));
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
        public ActionResult<VendaTransacaoOutputDto> CadastrarVenda([FromBody] VendaTransacaoCreateDto dto)
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
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<VendaTransacaoOutputDto> AtualizarVenda([FromRoute]Guid id, [FromBody] VendaTransacaoUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return Ok(_vendaService.AtualizarVenda(id, dto));
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeletarVendaPorId([FromRoute] Guid id)
        {
            try
            {
                _vendaService.DeletarVendaPorId(id);
                return Ok("Operação Realizada com Sucesso !!");
            }
            catch(ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpPatch("{idTransacao}")]
        [Authorize]
        //so retorno a transação ou a venda também??,  receber o id da transação ou da venda???
        //fica dificl pro front enviar o id da transação???
        public ActionResult<TransacaoOutputDto> AtualizarQuantidadeDeParcelasPagasEmUmTransacao([FromRoute]Guid idTransacao, [FromBody]AtualizarQuantidadeDeParcelasPagasInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                TransacaoOutputDto transacaoASerRetornada= _vendaService.AtualizarQuantidadeDeParcelasPagasEmUmaTransacao(idTransacao,dto.QuantidadeDeParcelasASerAtualizadaParaPaga);
                return Ok(transacaoASerRetornada);
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

        [HttpPost("relatorio-de-vendas-por-periodo")]
        [Authorize]
        public ActionResult<byte[]> GerarRelatoriosDeVendaPorPeriodo([FromBody]DatasParaGeracaoDeRelatorioDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                byte[] bytesPdf= _vendaService.GerarRelatorioDeVendasPorPeriodo(dto);
                return File(bytesPdf, "application/pdf", "relatorioDeVendasPorPeriodo.pdf");
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
        [HttpGet("buscar-vendas-por-documento-indentificador-do-cliente")]
        [Authorize]
        public ActionResult<List<VendaTransacaoOutputDto>> BuscarVendasPorCpfOuCnpj([FromBody] DocumentoClienteInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return _vendaService.BuscarVendasPorCpfOuCnpj(dto);
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpGet("buscar-venda-por-codigo-venda/{codigoVenda}")]
        [Authorize]
        public ActionResult<VendaTransacaoOutputDto> BuscarVendaPorCodigoVenda([FromRoute]string codigoVenda)
        {
            try
            {
                return _vendaService.BuscarVendaAtivaOuInativaPorCodigoVenda(codigoVenda);
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
        [HttpGet("log/{idVenda}")]
        public ActionResult<List<Object>> BuscarLogsPorIdVenda(Guid idVenda)
        {
            try
            {
                return Ok(_vendaService.BuscarLogsPorIdVenda(idVenda));
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
        [HttpGet("log/codigo-venda/{codigoVenda}")]
        public ActionResult<List<Object>> BuscarLogsPorCodigoVenda(string codigoVenda)
        {
            try
            {
                return Ok(_vendaService.BuscarLogsPorCodigoVenda(codigoVenda));
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
