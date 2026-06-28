using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Dtos.ServicoDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicoController : ControllerBase
    {
        private IServicoService _servicoService;
        public ServicoController(IServicoService servicoService) {
            this._servicoService = servicoService;
        }
        [HttpGet]
        [Authorize]
        public ActionResult<List<ServicoDtoOutPut>> BuscarServicos()
        {
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
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }
        [HttpGet("inativos")]
        [Authorize]
        public ActionResult<List<ServicoInativoOutputDto>> BuscarServicosInativos()
        {
            try
            {
                return _servicoService.BuscarServicosInativos();
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
        public ActionResult<ServicoDtoOutPut> BuscarServicoPorId([FromRoute] Guid id)
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
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }
        //mudar para codigo do serviço em tudo ou deixa como esta?
        [HttpGet("busca-por-codigo-do-servico/{codigoDoServico}")]
        [Authorize]
        public ActionResult<ServicoDtoOutPut> BuscarServicoPorCodigoDoServico([FromRoute] string codigoDoServico)
        {
            try
            {
                return _servicoService.BuscarServicoPorCodigoDoServico(codigoDoServico);
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
        public ActionResult<Servico> CadastrarServico([FromBody] ServicoInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                Servico servico= _servicoService.CadastrarServico(dto);
                return Created($"api/servico/{servico.Id}", servico);
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
        public ActionResult<Servico> AtualizarServico([FromRoute] Guid id, [FromBody] ServicoInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return _servicoService.AtualizarServico(id, dto);
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
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeletarServico([FromRoute] Guid id)
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
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpGet("buscar-servicos-por-nome/{nome}")]
        [Authorize]
        public ActionResult<List<ServicoDtoOutPut>> BuscarServicosPorNome([FromRoute] string nome)
        {
            try
            {
                return _servicoService.BuscarServicosPorNome(nome);
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
        [HttpGet("log/{idServico}")]
        public ActionResult<List<ServicoLogOutputDto>> BuscarLogsPorIdServico(Guid idServico)
        {
            try
            {
                return Ok(_servicoService.BuscarLogsPorIdServico(idServico));
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
        [HttpGet("log/codigo-do-servico/{codigoDoServico}")]
        public ActionResult<List<ServicoLogOutputDto>> BuscarLogsPorCodigoDoServico(string codigoDoServico)
        {
            try
            {
                return Ok(_servicoService.BuscarLogsPorCodigoDoServico(codigoDoServico));
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
