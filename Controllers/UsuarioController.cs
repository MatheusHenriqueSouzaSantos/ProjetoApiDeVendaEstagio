using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
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
    public class UsuarioController : ControllerBase
    {
        private IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this._usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<UsuarioOutputDto>> BuscarTodos()
        {
            try
            {
                return Ok(_usuarioService.BuscarTodosAtivos());
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
        [Authorize(Roles = "Admin")]
        public ActionResult<List<UsuarioOutputDto>> BuscarTodosInativos()
        {
            try
            {
                return Ok(_usuarioService.BuscarTodosInativos());
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
        [Authorize(Roles = "Admin")]
        public ActionResult<UsuarioOutputDto> BuscarPorId([FromRoute] Guid id)
        {
            try
            {
                return Ok(_usuarioService.BuscarPorIdAtivo(id));
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

        [HttpGet("me")]
        [Authorize]
        public ActionResult<UsuarioOutputDto> BuscarUsuarioLogado()
        {
            try
            {
                return Ok(_usuarioService.BuscarUsuarioLogado());
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
        [Authorize(Roles = "Admin")]
        public ActionResult<UsuarioOutputDto> Cadastrar([FromBody] UsuarioInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                UsuarioOutputDto usuarioDto = _usuarioService.Cadastrar(dto);

                return Created($"api/usuario/{usuarioDto.Id}", usuarioDto);
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
        [Authorize(Roles = "Admin")]
        public ActionResult<UsuarioOutputDto> Atualizar([FromRoute] Guid id, [FromBody] UsuarioInputDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return Ok(_usuarioService.Atualizar(id, dto));
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

        [HttpPut("me")]
        [Authorize]
        public ActionResult<UsuarioOutputDto> AtualizarUsuarioLogado([FromBody] AlteracaoDeUsuarioLogadoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    return BadRequest(mensagensDeErro);
                }
                return Ok(_usuarioService.AtualizarUsuarioLogado(dto));
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
        [Authorize(Roles = "Admin")]
        public ActionResult Inativar([FromRoute] Guid id)
        {
            try
            {
                _usuarioService.Inativar(id);
                return Ok("Operação Realizada Com Sucesso");
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

        [HttpPatch("reativar/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Reativar([FromRoute] Guid id)
        {
            try
            {
                _usuarioService.Reativar(id);
                return Ok("Operação Realizada Com Sucesso");
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

        [HttpPost("login")]
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
                return StatusCode(500, "Erro Inesperado, entre em contato com o suporte");
            }
        }

        [HttpGet("log/{idUsuario}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<UsuarioLogOutputDto>> BuscarLogsPorIdUsuario(Guid idUsuario)
        {
            try
            {
                return Ok(_usuarioService.BuscarLogsPorIdUsuario(idUsuario));
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
        [HttpGet("log/codigo-usuario/{codigoUsuario}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<UsuarioLogOutputDto>> BuscarLogsPorIdCodigoUsuario(string codigoUsuario)
        {
            try
            {
                return Ok(_usuarioService.BuscarLogsPorCodigoUsuario(codigoUsuario));
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
