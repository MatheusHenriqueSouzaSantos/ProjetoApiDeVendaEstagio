using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class ProdutoController : ControllerBase
    {
        private IProdutoService _produtoService;
        public ProdutoController(IProdutoService produtoService) {
            this._produtoService = produtoService;
        }

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

        public ActionResult<Produto> BuscarProdutoPorId([FromRoute] Guid id)
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
        public ActionResult<Produto> BuscarProdutoPorCodigoDeBarra([FromRoute] string codigoDeBarra)
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

        public ActionResult<Produto> CadastrarProduto([FromBody]ProdutoDto dto)
        {
            try
            {
                return _produtoService.CadastrarProduto(dto);
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
        public ActionResult<Produto> AtualizarProduto([FromRoute] Guid id, [FromBody] ProdutoDto dto)
        {
            try
            {
                return _produtoService.AtualizarProduto(id,dto);
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

        public ActionResult DeletarProduto([FromRoute] Guid id)
        {
            try
            {
                return _produtoService.DeletarProdutoPorId(id);
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
    }
}
