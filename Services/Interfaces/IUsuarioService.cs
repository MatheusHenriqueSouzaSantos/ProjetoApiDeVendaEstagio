using ApiEstagioBicicletaria.Dtos.Usuario;
using ApiEstagioBicicletaria.Dtos.UsuarioDtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IUsuarioService
    {
        // o usuario inicial sempre será criado pelo sistema
        List<UsuarioOutputDto> BuscarTodos();

        UsuarioOutputDto BuscarPorId(Guid id);

        UsuarioOutputDto Cadastrar(UsuarioInputDto dto);

        UsuarioOutputDto Atualizar(Guid id, UsuarioInputDto dto);

        UsuarioOutputDto AtualizarUsuarioLogado(UsuarioLogadoInputDto dto);

        void Desativar(Guid id);

        string Login(UsuarioLoginDto dto);

        List<UsuarioLogOutputDto> BuscarLogsPorIdUsuario(Guid id);
    }
}
