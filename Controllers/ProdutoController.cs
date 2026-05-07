using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private IProdutoService _produtoService;
        public ProdutoController(IProdutoService produtoService) {
            this._produtoService = produtoService;
        }
        [HttpGet]
        [Authorize]
        public ActionResult<List<ProdutoDtoOutPut>> BuscarProdutos()
        {
            
            try
            {
                return _produtoService.BuscarProdutos();
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
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
        public ActionResult<ProdutoDtoOutPut> BuscarProdutoPorId([FromRoute] Guid id)
        {
            try
            {
                return _produtoService.BuscarProdutoPorId(id);
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
        [HttpGet("busca-por-codigo-de-barra/{codigoDeBarra}")]
        [Authorize]
        public ActionResult<ProdutoDtoOutPut> BuscarProdutoPorCodigoDeBarra([FromRoute] string codigoDeBarra)
        {
            try
            {
                return _produtoService.BuscarProdutoPorCodigoDeBarra(codigoDeBarra);
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
        [Authorize]
        public ActionResult<Produto> CadastrarProduto([FromBody] ProdutoInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                Produto produto= _produtoService.CadastrarProduto(dto);
                return Created($"api/produto/{produto.Id}",produto);
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
        [Authorize]
        public ActionResult<Produto> AtualizarProduto([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id, [FromBody] ProdutoInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return _produtoService.AtualizarProduto(id, dto);
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
        [Authorize]
        public ActionResult DeletarProduto([FromRoute] Guid id)
        {
            try
            {
                _produtoService.DeletarProdutoPorId(id);
                return Ok("Operação realizada com sucesso ");
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
        [HttpGet("buscar-produtos-por-nome/{nome}")]
        [Authorize]
        public ActionResult<List<ProdutoDtoOutPut>> BuscarProdutosPorNome([FromRoute] string nome)
        {
            try
            {
                return _produtoService.BuscarProdutosPorNome(nome);
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

        [HttpPost("relatorio-produtos-com-maior-faturamento-por-periodo")]
        [Authorize]
        public ActionResult<byte[]> GerarRelatorioDeProdutosComMaiorFaturamento(DatasParaGeracaoDeRelatorioDto dto)
        {
            try
            {
                byte[] bytesPdf= _produtoService.GerarRelatorioDeProdutosComMaiorFaturamentoPorPeriodo(dto);
                return File(bytesPdf, "application/pdf", "relatorioDeProdutosComMaiorFaturamentoPorPeriodo.pdf");
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //não retornar a mensagem pois indica exatamente o erro e há o risco de ameaças explorarem
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }
        }

        [HttpGet("relatorio-de-produtos-em-falta/{quantidadeParaBuscarDosProdutosEmFalta}")]
        [Authorize]
        public ActionResult<byte[]> GerarRelatorioDeProdutosEmFalta(int quantidadeParaBuscarDosProdutosEmFalta)
        {
            try
            {
                byte[] bytesPdf = _produtoService.GerarRelatorioDeProdutosComEstoqueAbaixoOuIgualUmaQuantidade(quantidadeParaBuscarDosProdutosEmFalta);
                return File(bytesPdf, "application/pdf", "relatorioDeProdutosEmFalta.pdf");
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //não retornar a mensagem pois indica exatamente o erro e há o risco de ameaças explorarem
                //return StatusCode(500, ex.Message);
                return StatusCode(500, "Erro Inesperado");
            }
        }
    }
}
