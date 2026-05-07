using ApiEstagioBicicletaria.Dtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.Interfaces;

namespace ApiEstagioBicicletaria.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ContextoDb _contextoDb;

        private readonly ServicoJwt _servicoJwt;

        public UsuarioService(ContextoDb contextoDb, ServicoJwt servicoJwt)
        {
            _contextoDb = contextoDb;
            _servicoJwt = servicoJwt;
        }

        //fazer a criação do end point para criar usuario

        public string Login(UsuarioDto dto)
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
            return _servicoJwt.GerarJWT(usuarioVindoDoBanco);
        }
    }
}
