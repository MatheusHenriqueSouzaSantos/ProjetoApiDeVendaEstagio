using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Security.Claims;

namespace ApiEstagioBicicletaria.Seguranca
{
    public class UsuarioLogadoService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ContextoDb _contextoDb;

        public UsuarioLogadoService(IHttpContextAccessor httpContext, ContextoDb contextoDb)
        {
            _httpContext = httpContext;
            _contextoDb = contextoDb;
        }

        public Usuario ObterUsuario()
        {
            string idUserEmString = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                throw new ExcecaoDeRegraDeNegocio(500,"Não foi encontrado o usuario logado");
            if(!Guid.TryParse(idUserEmString, out Guid id))
            {
                throw new ExcecaoDeRegraDeNegocio(500, "Erro interno de conversão");
            }

            Usuario usuarioVindoDoBanco= _contextoDb.Usuarios.FirstOrDefault(u=>u.Id==id) 
                ?? throw new ExcecaoDeRegraDeNegocio(500, "Não foi encontrado o usuario logado");
            if (usuarioVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(403, "Não é possível concluir a ação, pois o usuário está inativo");
            }
            return usuarioVindoDoBanco;
        }
    }
}
