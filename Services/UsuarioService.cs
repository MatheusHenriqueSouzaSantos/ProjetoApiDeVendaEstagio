using ApiEstagioBicicletaria.Dtos.Usuario;
using ApiEstagioBicicletaria.Dtos.UsuarioDtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.Interfaces;

namespace ApiEstagioBicicletaria.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepositorio _repositorio;

        private readonly ServicoJwt _servicoJwt;

        private readonly SenhaService _senhaService;

        private readonly ContextoDb _contexto;

        public UsuarioService(UsuarioRepositorio repositorio, ServicoJwt servicoJwt, SenhaService senhaService, ContextoDb contexto)
        {
            _repositorio = repositorio;
            _servicoJwt = servicoJwt;
            _senhaService = senhaService;
            _contexto = contexto;
        }

        public List<UsuarioOutputDto> BuscarTodos()
        {
            return _repositorio.BuscarTodos().Select(EntidadeParaDto).ToList();
        }

        public UsuarioOutputDto BuscarPorId(Guid id)
        {
            Usuario usuario = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            return EntidadeParaDto(usuario);
        }

        public UsuarioOutputDto Cadastrar(UsuarioInputDto dto)
        {
            if (_repositorio.UsuarioExistentePorEmail(dto.Email))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "já existe um usuário cadastrado com esse email");
            }
            string hashSenha=_senhaService.GerarHashDaSenha(dto.Senha);
            Usuario usuario = new(dto.Nome, dto.Email, hashSenha);
            _repositorio.Cadastrar(usuario);
            _contexto.SaveChanges();
            return EntidadeParaDto(usuario);
        }

        public UsuarioOutputDto Atualizar(Guid id, UsuarioInputDto dto)
        {
            Usuario usuarioVindoDoBanco = _repositorio.BuscarPorId(id)
               ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            Usuario? usuarioVindoDoBancoComEmailInformado = _repositorio.BuscarPorEmail(dto.Email);
            //usuario pode ter esse email, se for o próprio usuario e ele não for alterar o email
            if(usuarioVindoDoBancoComEmailInformado !=null  && usuarioVindoDoBancoComEmailInformado.Id != id)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Já existe um usuário com esse email cadastrado");
            }
            usuarioVindoDoBanco.Nome = dto.Nome;
            usuarioVindoDoBanco.Email=dto.Email;
            usuarioVindoDoBanco.Senha = _senhaService.GerarHashDaSenha(dto.Senha);

            _repositorio.Atualizar(usuarioVindoDoBanco);
            _contexto.SaveChanges();
            return EntidadeParaDto(usuarioVindoDoBanco);
        }

        public void Desativar(Guid id)
        {
            Usuario usuarioVindoDoBanco = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuaário não encontrado");

            usuarioVindoDoBanco.Ativo = false;
            _repositorio.Atualizar(usuarioVindoDoBanco);
            _contexto.SaveChanges();
        }

        public string Login(UsuarioLoginDto dto)
        {
            Usuario? usuarioVindoDoBanco = _repositorio.BuscarPorEmail(dto.Email);
            if(usuarioVindoDoBanco==null)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"usuário ou senha inválida");
            }
            if (!_senhaService.ValidarSenha(usuarioVindoDoBanco.Senha,dto.Senha))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "usuário ou senha inválida");
            }
            var jwt= _servicoJwt.GerarJWT(usuarioVindoDoBanco);
            return jwt;
        }

        private UsuarioOutputDto EntidadeParaDto(Usuario usuario)
        {
            return new UsuarioOutputDto(usuario.Id, usuario.DataCriacao, usuario.Ativo, usuario.Nome, usuario.Email);
        }

    }
}
