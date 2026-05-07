using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
//revisar e tirar as funções de adicionar e remover quantidade em estoque, pois esse é o objetivo de entrada estoque
namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private IEstoqueService _service;

        public EstoqueController(IEstoqueService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<EstoqueSimplificadoOutputDto> BuscarPorId([FromRoute]Guid id)
        {
            try
            {
                return _service.BuscarPorId(id);
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

        [HttpPatch("{id}/adicionar-quantidade-em-estoque/{quantidade}")]
        [Authorize]
        public ActionResult<Produto> AdicionarQuantidadeEmEstoqueDeProdutoPorId([FromRoute]Guid id,[FromRoute] int quantidade)
        {
            try
            {
                return Ok(_service.AdicionarQuantidadeEmEstoque(id, quantidade));
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

        [HttpPatch("{id}/abater-quantidade-em-estoque/{quantidade}")]
        [Authorize]
        public ActionResult<Produto> AbaterQuantidadeEmEstoqueDeProdutoPorId([FromRoute] Guid id,[FromRoute] int quantidade)
        {
            try
            {
                return Ok(_service.AbaterQuantidadeEmEstoque(id, quantidade));
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
