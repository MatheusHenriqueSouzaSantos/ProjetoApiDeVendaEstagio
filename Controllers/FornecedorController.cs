using ApiEstagioBicicletaria.Dtos.ClienteDtos;
using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Dtos.RelatorioDtos;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
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
    public class FornecedorController : ControllerBase
    {
        private IFornecedorService _service;

        public FornecedorController(IFornecedorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Fornecedor>> BuscarTodos() {
            try
            {
                return _service.BuscarTodos();
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
        public ActionResult<List<Fornecedor>> BuscarTodosInativos()
        {
            try
            {
                return _service.BuscarTodosInativos();
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
        public ActionResult<Fornecedor> BuscarPorId([FromRoute]Guid id)
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
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpGet("buscar-por-cnpj/{cnpj}")]
        [Authorize]
        public ActionResult<Fornecedor> BuscarPorCnpj([FromRoute] string cnpj)
        {
            try
            {
                return _service.BuscarPorCnpj(cnpj);
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

        [HttpGet("buscar-por-nome/{nome}")]
        [Authorize]
        public ActionResult<List<Fornecedor>> BuscarFornecedorPorNome([FromRoute] string nome)
        {
            try
            {
                return Ok(_service.BuscarPorNome(nome));
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
        public ActionResult<Fornecedor> Cadastrar(FornecedorCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                Fornecedor fornecedor = _service.Cadastrar(dto);
                return Created($"api/vendedor/{fornecedor.Id}", fornecedor);
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
        public ActionResult<Fornecedor> Atualizar([FromRoute, Required(ErrorMessage = "O id é obrigatório")] Guid id, [FromBody] FornecedorUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return _service.Atualizar(id, dto);
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
        public ActionResult Desativar([FromRoute] Guid id)
        {
            try
            {
                _service.Desativar(id);
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

        [HttpPost("gerar-relatorio-fornecedores-com-maior-volume-de-entrada-por-periodo")]
        [Authorize]
        public ActionResult<byte[]> GerarRelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo([FromBody]DatasParaGeracaoDeRelatorioDto dto)
        {
            try
            {
                byte[] bytesPdf = _service.GerarRelatorioFornecedoresComMaiorVolumeDeEntradaPorPeriodo(dto);
                return File(bytesPdf, "application/pdf", "relatorioFornecedoresComMaiorVolumeEntradaPorPeriodo.pdf");
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
        [HttpGet("log/{idFornecedor}")]
        public ActionResult<List<FornecedorLogOutputDto>> BuscarLogsPorIdFornecedor(Guid idFornecedor)
        {
            try
            {
                return Ok(_service.BuscarLogsPorIdFornecedor(idFornecedor));
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
        [HttpGet("log/documento-identificador/{cnpj}")]
        public ActionResult<List<FornecedorLogOutputDto>> BuscarLogsPorCnpj(string cnpj)
        {
            try
            {
                return Ok(_service.BuscarLogsPorCnpj(cnpj));
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
