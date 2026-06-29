using ApiEstagioBicicletaria.Dtos.Usuario;
using ApiEstagioBicicletaria.Dtos.UsuarioDtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;

namespace ApiEstagioBicicletaria.Services.Interfaces
{
    public interface IUsuarioService
    {
        // o usuario inicial sempre será criado pelo sistema
        List<UsuarioOutputDto> BuscarTodosAtivos();
        List<UsuarioOutputDto> BuscarTodosInativos();

        UsuarioOutputDto BuscarPorIdAtivo(Guid id);

        UsuarioOutputDto BuscarUsuarioLogado();

        UsuarioOutputDto Cadastrar(UsuarioInputDto dto);

        UsuarioOutputDto Atualizar(Guid id, UsuarioInputDto dto);

        UsuarioOutputDto AtualizarUsuarioLogado(AlteracaoDeUsuarioLogadoDto dto);

        void Inativar(Guid id);

        void Reativar(Guid id);

        string Login(UsuarioLoginDto dto);

        List<UsuarioLogOutputDto> BuscarLogsPorIdUsuario(Guid id);

        List<UsuarioLogOutputDto> BuscarLogsPorCodigoUsuario(string codigoUsuario);
    }
}
