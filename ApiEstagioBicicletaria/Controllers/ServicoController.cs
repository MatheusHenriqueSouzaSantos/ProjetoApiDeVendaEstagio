using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class ServicoController : ControllerBase
    {
        private IServicoService _servicoService;
        public ServicoController(IServicoService servicoService) {
            this._servicoService = servicoService;
        }
        [HttpGet]
        public ActionResult<List<Servico>> BuscarServicos()
        {
            //if (!ModelState.IsValid)
            try
            {
                return _servicoService.BuscarServicos();
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
        public ActionResult<Servico> BuscarServicoPorId([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id)
        {
            try
            {
                return _servicoService.BuscarServicoPorId(id);
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
        //mudar para codigo do serviço em tudo ou deixa como esta?
        [HttpGet("busca-por-codigo-do-servico/{codigoDoServico}")]
        public ActionResult<Servico> BuscarProdutoPorCodigoDeBarra([FromRoute, Required(ErrorMessage = "O Código do Serviço é obrigatório")] string codigoDoServico)
        {
            try
            {
                return _servicoService.BuscarServicoPorCodigoServico(codigoDoServico);
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
        public ActionResult<Servico> CadastrarServico([FromBody] ServicoDto dto)
        {
            try
            {
                Servico servico= _servicoService.CadastrarServico(dto);
                return Created($"api/servico/{servico.Id}", servico);
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
        [HttpPut]
        public ActionResult<Servico> AtualizarServico([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id, [FromBody] ServicoDto dto)
        {
            try
            {
                return _servicoService.AtualizarServico(id, dto);
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
        [HttpDelete]
        public ActionResult DeletarServico([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id)
        {
            try
            {
                _servicoService.DeletarServicoPorId(id);
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
       
    }
}
