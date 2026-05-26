using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.ClassesDeGeracaoDeRelatorios;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class VendedorController :  ControllerBase
    {
        private IVendedorService _service;

        public VendedorController(IVendedorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Vendedor>> BuscarTodosVendedores()
        {
            try
            {
                return Ok(_service.BuscarTodosOsVendedores());
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500,"Erro Inesperado");
            }

        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<List<Vendedor>> BuscarVendedorPorId([FromRoute]Guid id)
        {
            try
            {
                return Ok(_service.BuscarVendedorPorId(id));
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

        [HttpGet("buscar-por-cpf/{cpf}")]
        [Authorize]
        public ActionResult<List<Vendedor>> BuscarVendedorPorCpf([FromRoute]string cpf)
        {
            try
            {
                return Ok(_service.BuscarVendedorPorCpf(cpf));
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

        [HttpGet("buscar-por-nome/{nome}")]
        [Authorize] 
        public ActionResult<List<Vendedor>> BuscarVendedorPorNome([FromRoute]string nome)
        {
            try
            {
                return Ok(_service.BuscarVendedoresPorNome(nome));
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
        public ActionResult<Vendedor> CadastrarVendedor([FromBody] VendedorCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                Vendedor vendedor = _service.CadastrarVendedor(dto);
                return Created($"api/vendedor/{vendedor.Id}", vendedor);
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
        public ActionResult<Vendedor> AtualizarVendedor([FromRoute]Guid id,[FromBody] VendedorUpdatedDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }

                return Ok(_service.AtualizarVendedor(id, dto));
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
        public ActionResult<Vendedor> Desativar([FromRoute]Guid id)
        {
            try
            {
                _service.DesativarVendedor(id);

                return NoContent();
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
        [Authorize]
        [HttpPost("relatorio-de-vendedores-com-maior-faturamento-por-periodo")]
        public ActionResult<byte[]> GerarRelatorioDeVendedoresComMaiorFaturamentoPorPeriodo
            (DatasParaGeracaoDeRelatorioDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                byte[] bytesPdf = _service.GerarRelatorioDeVendedoresComMaiorFaturamentoPorPeriodo(dto);
                return File(bytesPdf, "application/pdf", "RelatorioDeVendedoresQueMaisRealizaramVendasPorPeriodo");
            }
            catch (ExcecaoDeRegraDeNegocio ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, "Erro Inesperado");
                return StatusCode(500, ex.Message);

            }
        }
    }
}
