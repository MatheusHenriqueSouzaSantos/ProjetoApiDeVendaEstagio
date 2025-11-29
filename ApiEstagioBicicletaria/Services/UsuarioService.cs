using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Services.Interfaces;

namespace ApiEstagioBicicletaria.Services
{
    public class UsuarioService : IUsuarioService
    {
        private ContextoDb _contextoDb;

        public UsuarioService(ContextoDb contextoDb)
        {
            this._contextoDb = contextoDb;
        }

        public bool ValidarUsuario(UsuarioDto dto)
        {
            Usuario? usuarioVindoDoBanco= _contextoDb.Usuarios.Where(u=>u.Email==dto.Email).FirstOrDefault();

            if(usuarioVindoDoBanco==null)
            {
                throw new ExcecaoDeRegraDeNegocio(404,"Usuário não existe");
            }
            if (usuarioVindoDoBanco.Senha != dto.Senha)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Senha inválida");
            }
            return true;
        }
    }
}
