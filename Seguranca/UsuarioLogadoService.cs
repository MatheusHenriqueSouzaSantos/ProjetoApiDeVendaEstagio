using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Security.Claims;

namespace ApiEstagioBicicletaria.Seguranca
{
    public class UsuarioLogadoService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public UsuarioLogadoService(IHttpContextAccessor httpContext, UsuarioRepositorio usuarioRepositorio)
        {
            _httpContext = httpContext;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Usuario ObterUsuario()
        {
            string idUserEmString = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                throw new ExcecaoDeRegraDeNegocio(500,"Não foi encontrado o usuario logado");
            if(!Guid.TryParse(idUserEmString, out Guid id))
            {
                throw new ExcecaoDeRegraDeNegocio(500, "Erro interno de conversão");
            }

            return _usuarioRepositorio.BuscarPorId(id) ?? throw new ExcecaoDeRegraDeNegocio(500, "Não foi encontrado o usuario logado"); ;
        }
    }
}
