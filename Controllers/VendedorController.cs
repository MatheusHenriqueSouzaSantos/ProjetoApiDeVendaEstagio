using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services.Interfaces;
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
                return StatusCode(500, "Erro Inesperado");
            }

        }

        [HttpGet("{id}")]
        public ActionResult<List<Vendedor>> BuscarVendedorPorId(Guid id)
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

        [HttpGet("buscar-por-cpf{cpf}")]
        public ActionResult<List<Vendedor>> BuscarVendedorPorCpf(string cpf)
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

        [HttpGet("buscar-por-nome{nome}")]
        public ActionResult<List<Vendedor>> BuscarVendedorPorNome(string nome)
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
        public ActionResult<Vendedor> CriarVendedor([FromBody] VendedorCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }

                return Ok(_service.CriarVendedor(dto));
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
        public ActionResult<Vendedor> AtualizarVendedor(Guid id,[FromBody] VendedorUpdatedDto dto)
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
        public ActionResult<Vendedor> Desativar(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }

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
    }
}
