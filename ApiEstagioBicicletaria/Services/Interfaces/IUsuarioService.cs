using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IUsuarioService
    {
        //criar users por enquanto direto no banco ou aplicação
        //depois no lugar de bool retornar jwt
        //se não lançar erro esta certo, ou retornar um bool
        //ou retornar um bool para dizer se esta válido explicitamente?
        bool ValidarUsuario(UsuarioDto dto);
    }
}
