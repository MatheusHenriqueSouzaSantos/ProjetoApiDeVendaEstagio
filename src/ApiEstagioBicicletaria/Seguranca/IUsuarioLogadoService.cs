using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Seguranca
{
    public interface IUsuarioLogadoService
    {
        Usuario ObterUsuario();
    }
}
