using ApiEstagioBicicletaria.Dtos.Usuario;
using ApiEstagioBicicletaria.Dtos.UsuarioDtos;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController:ControllerBase
    {
        private IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this._usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<UsuarioOutputDto>> BuscarTodos()
        {
            try
            {
                return Ok(_usuarioService.BuscarTodos());
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

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UsuarioOutputDto> BuscarPorId([FromRoute] Guid id)
        {
            try
            {
                return Ok(_usuarioService.BuscarPorId(id));
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
        public ActionResult<UsuarioOutputDto> Cadastrar([FromBody]UsuarioInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                UsuarioOutputDto usuarioDto=_usuarioService.Cadastrar(dto);

                return Created($"api/usuario/{usuarioDto.Id}", usuarioDto);
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
        [HttpPut("/{id}")]
        [Authorize]
        public ActionResult<UsuarioOutputDto> Atualizar([FromRoute]Guid id, [FromBody] UsuarioInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return Ok(_usuarioService.Atualizar(id,dto));
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
        [Authorize]
        public ActionResult Desativar([FromRoute] Guid id)
        {
            try
            {
                _usuarioService.Desativar(id);
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

        [HttpPost("/login")]
        public ActionResult Login(UsuarioLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(mensagensDeErro);
            }
            try
            {
                string tokenJWT=_usuarioService.Login(dto);
                return Ok(new { TokenJWT = tokenJWT });
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
