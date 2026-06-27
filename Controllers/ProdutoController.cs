using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
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

        [HttpGet("inativos")]
        [Authorize]
        public ActionResult<List<ProdutoInativoOutputDto>> BuscarProdutosInativos()
        {

            try
            {
                return _produtoService.BuscarProdutosInativos();
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

        [HttpPost("relatorio-produtos-mais-vendidos-por-periodo")]
        [Authorize]
        public ActionResult<byte[]> GerarRelatorioDeProdutosMaisVendidosPorPeriodo(DatasParaGeracaoDeRelatorioDto dto)
        {
            try
            {
                byte[] bytesPdf= _produtoService.GerarRelatorioDeProdutosMaisVendidosPorPeriodo(dto);
                return File(bytesPdf, "application/pdf", "relatorioDeProdutosMisVendidosPorPeriodo.pdf");
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

        [Authorize(Roles = "Admin")]
        [HttpGet("log/{idProduto}")]
        public ActionResult<List<Object>> BuscarLogsPorIdProduto(Guid idProduto)
        {
            try
            {
                return Ok(_produtoService.BuscarLogsPorIdProduto(idProduto));
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

        [Authorize(Roles = "Admin")]
        [HttpGet("log/codigo-de-barra/{codigoDeBarra}")]
        public ActionResult<List<Object>> BuscarLogsPorCodigoDeBarra(string codigoDeBarra)
        {
            try
            {
                return Ok(_produtoService.BuscarLogsPorCodigoDeBarra(codigoDeBarra));
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

        //[Authorize]
        //[HttpGet("log-geral-paginacao")]
        //public ActionResult<List<ProdutoLogDto>> BuscarLogsGeraisPaginacao([FromQuery][Required(ErrorMessage ="é obrigatório enviar a página a ser buscada")] int pagina,
        //   [FromQuery][Required(ErrorMessage = "é obrigatório enviar a quantidade de registro por página a ser buscada")] int quantidadeDeRegistroPorPagina)
        //{
        //    try
        //    {
        //        return Ok(_produtoService.BuscarLogsPorIdProduto(idProduto));
        //    }
        //    catch (ExcecaoDeRegraDeNegocio ex)
        //    {
        //        return StatusCode(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Erro Inesperado");
        //        //return StatusCode(500, ex.Message);

        //    }
        //}
    }
}
