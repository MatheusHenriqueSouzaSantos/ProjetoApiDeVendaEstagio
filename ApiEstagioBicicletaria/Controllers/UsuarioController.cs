using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Controllers
{
    [ApiController]
    [Route("/api[controller]")]
    public class UsuarioController :ControllerBase
    {
        private IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this._usuarioService = usuarioService;
        }
        [HttpPost]
        public ActionResult ValidarUsuario(UsuarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                var mensagensDeErro = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(mensagensDeErro);
            }
            try
            {
                bool resultadoValidacao=_usuarioService.ValidarUsuario(dto);
                if (!resultadoValidacao)
                {
                    throw new ExcecaoDeRegraDeNegocio(400,"Email ou senha inválida");
                }
                return Ok(new{valido=true});
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
