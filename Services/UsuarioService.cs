using ApiEstagioBicicletaria.Dtos.Usuario;
using ApiEstagioBicicletaria.Dtos.UsuarioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;

namespace ApiEstagioBicicletaria.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepositorio _repositorio;

        private readonly ServicoJwt _servicoJwt;

        private readonly SenhaService _senhaService;

        private readonly ContextoDb _contexto;

        private readonly UsuarioLogService _usuarioLogService;

        private readonly UsuarioLogadoService _usuarioLogadoService;

        public UsuarioService(UsuarioRepositorio repositorio, ServicoJwt servicoJwt, SenhaService senhaService, ContextoDb contexto,
            UsuarioLogService usuarioLogService,UsuarioLogadoService usuarioLogadoService)
        {
            _repositorio = repositorio;
            _servicoJwt = servicoJwt;
            _senhaService = senhaService;
            _contexto = contexto;
            _usuarioLogService = usuarioLogService;
            _usuarioLogadoService = usuarioLogadoService;
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
            Usuario usuario = new(dto.Nome, dto.Email, hashSenha,dto.PerfilUsuario);
            _repositorio.Cadastrar(usuario);
            _usuarioLogService.CriarLogDeCriacao(usuario, _usuarioLogadoService.ObterUsuario());
            _contexto.SaveChanges();
            return EntidadeParaDto(usuario);
        }

        public UsuarioOutputDto Atualizar(Guid id, UsuarioInputDto dto)
        {
            Usuario usuarioVindoDoBanco = _repositorio.BuscarPorId(id)
               ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            Usuario? usuarioVindoDoBancoComEmailInformado = _repositorio.BuscarPorEmail(dto.Email);

            if(usuarioVindoDoBancoComEmailInformado !=null  && usuarioVindoDoBancoComEmailInformado.Id != id)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Já existe um usuário com esse email cadastrado");
            }
            Usuario usuarioCopia = usuarioVindoDoBanco.Copia();
            usuarioVindoDoBanco.Nome = dto.Nome;
            usuarioVindoDoBanco.Email=dto.Email;
            usuarioVindoDoBanco.Senha = _senhaService.GerarHashDaSenha(dto.Senha);
            usuarioVindoDoBanco.PerfilUsuario = dto.PerfilUsuario;

            _repositorio.Atualizar(usuarioVindoDoBanco);
            _usuarioLogService.CriarLogsDeAtualizacao(usuarioCopia, usuarioVindoDoBanco, _usuarioLogadoService.ObterUsuario());
            _contexto.SaveChanges();
            return EntidadeParaDto(usuarioVindoDoBanco);
        }

        public UsuarioOutputDto AtualizarUsuarioLogado(UsuarioLogadoInputDto dto)
        {
            Usuario usuarioLogado = _usuarioLogadoService.ObterUsuario();

            Usuario? usuarioVindoDoBancoComEmailInformado = _repositorio.BuscarPorEmail(dto.Email);

            if (usuarioVindoDoBancoComEmailInformado != null && usuarioVindoDoBancoComEmailInformado.Id != usuarioLogado.Id)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um usuário com esse email cadastrado");
            }
            Usuario usuarioCopia = usuarioLogado.Copia();
            usuarioLogado.Nome = dto.Nome;
            usuarioLogado.Email = dto.Email;
            usuarioLogado.Senha = _senhaService.GerarHashDaSenha(dto.Senha);

            _repositorio.Atualizar(usuarioLogado);
            _usuarioLogService.CriarLogsDeAtualizacao(usuarioCopia, usuarioLogado, usuarioLogado);
            _contexto.SaveChanges();
            return EntidadeParaDto(usuarioLogado);
        }

        public void Desativar(Guid id)
        {
            Usuario usuarioVindoDoBanco = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuaário não encontrado");

            usuarioVindoDoBanco.Ativo = false;
            _repositorio.Atualizar(usuarioVindoDoBanco);
            _usuarioLogService.CriarLogsDeExclusao(usuarioVindoDoBanco, _usuarioLogadoService.ObterUsuario());
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

        public List<UsuarioLogOutputDto> BuscarLogsPorIdUsuario(Guid id)
        {
            List<UsuarioLog> logs = _contexto.UsuarioLogs
                .Where(l => l.IdUsuario == id).ToList();

            List<UsuarioLogOutputDto> logsDto =
                logs.Select(l => new UsuarioLogOutputDto
                (l.IdUsuario,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            return logsDto.OrderByDescending(l => l.DataCriacao).ToList();
        }

        private UsuarioOutputDto EntidadeParaDto(Usuario usuario)
        {
            return new UsuarioOutputDto(usuario.Id, usuario.DataCriacao, usuario.Ativo, usuario.Nome, usuario.Email,usuario.PerfilUsuario);
        }

    }
}
