using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public ActionResult<EstoqueSimplificadoOutputDto> BuscarPorId(Guid id)
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
        public ActionResult<Produto> AdicionarQuantidadeEmEstoqueDeProdutoPorId(Guid id, int quantidade)
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
        public ActionResult<Produto> AbaterQuantidadeEmEstoqueDeProdutoPorId(Guid id, int quantidade)
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
