using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
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
        public ActionResult<List<Produto>> BuscarProdutos()
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
        public ActionResult<Produto> BuscarProdutoPorId([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
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
        public ActionResult<Produto> BuscarProdutoPorCodigoDeBarra([FromRoute, Required(ErrorMessage = "O Código de Barras é obrigatório")] string codigoDeBarra)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
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
        public ActionResult<Produto> CadastrarProduto([FromBody] ProdutoDto dto)
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
        public ActionResult<Produto> AtualizarProduto([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id, [FromBody] ProdutoDto dto)
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
        public ActionResult DeletarProduto([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
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

        [HttpPatch("{idProduto}/adicionar-quantidade-em-estoque/{quantidadeAAdicionarEmEstoque}")]
        public ActionResult<Produto> AdicionarQuantidadeEmEstoqueDeProdutoPorId(string idProduto,int quantidadeAAdicionarEmEstoque)
        {
            try
            {
                if(!Guid.TryParse(idProduto, out Guid idProdutoConvertido))
                {
                    return BadRequest("id no formato inválido de GUID");
                }

                return Ok(_produtoService.AdicionarQuantidadeEmEstoqueDeProdutoPorId(idProdutoConvertido, quantidadeAAdicionarEmEstoque));
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

        [HttpPatch("{idProduto}/abater-quantidade-em-estoque/{quantidadeAAbaterEmEstoque}")]
        public ActionResult<Produto> AbaterQuantidadeEmEstoqueDeProdutoPorId(string idProduto,int quantidadeAAbaterEmEstoque)
        {
            try
            {
                if (!Guid.TryParse(idProduto, out Guid idProdutoConvertido))
                {
                    return BadRequest("id no formato inválido de GUID");
                }

                return Ok(_produtoService.AdicionarQuantidadeEmEstoqueDeProdutoPorId(idProdutoConvertido, quantidadeAAbaterEmEstoque));
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

        //[HttpPatch("{id}/{quantidade}")]
        //public ActionResult DefinirQuantidadeEmEstoqueDeProduto([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id, [FromRoute, Required(ErrorMessage ="A quantidade é obrigatória")]int quantidade)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

        //            return BadRequest(mensagensDeErro);
        //        }
        //        _produtoService.DefinirQuantidadeEmEstoqueDeProduto(id,quantidade);
        //        return Ok("Operação realizada com sucesso ");
        //    }
        //    catch (ExcecaoDeRegraDeNegocio ex)
        //    {
        //        return StatusCode(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Erro Inesperado");
        //    }
        //}
        [HttpGet("buscar-produtos-por-nome/{nome}")]
        public ActionResult<List<Produto>> BuscarProdutosPorNome([FromRoute, Required(ErrorMessage = "O Nome é obrigatório")] string nome)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
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
