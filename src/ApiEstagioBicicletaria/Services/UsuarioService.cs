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
using ApiEstagioBicicletaria.Utils;
using Microsoft.EntityFrameworkCore;

namespace ApiEstagioBicicletaria.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepositorio _repositorio;

        private readonly ServicoJwt _servicoJwt;

        private readonly SenhaService _senhaService;

        private readonly ContextoDb _contexto;

        private readonly UsuarioLogService _usuarioLogService;

        private readonly IUsuarioLogadoService _usuarioLogadoService;
        private readonly GeradorCodigoIndentificador<Usuario> _geradorCodigoIndentificador;

        public UsuarioService(UsuarioRepositorio repositorio, ServicoJwt servicoJwt, SenhaService senhaService, ContextoDb contexto,
            UsuarioLogService usuarioLogService,IUsuarioLogadoService usuarioLogadoService,GeradorCodigoIndentificador<Usuario> geradorCodigoIndentificador)
        {
            _repositorio = repositorio;
            _servicoJwt = servicoJwt;
            _senhaService = senhaService;
            _contexto = contexto;
            _usuarioLogService = usuarioLogService;
            _usuarioLogadoService = usuarioLogadoService;
            _geradorCodigoIndentificador=geradorCodigoIndentificador;
        }

        public List<UsuarioOutputDto> BuscarTodosAtivos()
        {
            return _repositorio.BuscarTodos().Select(EntidadeParaDto).ToList();
        }

        public List<UsuarioOutputDto> BuscarTodosInativos()
        {
            return _contexto.Usuarios.Where(u=>!u.Ativo).Select(EntidadeParaDto).ToList();
        }

        public UsuarioOutputDto BuscarPorIdAtivo(Guid id)
        {
            Usuario usuario = _repositorio.BuscarPorId(id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            return EntidadeParaDto(usuario);
        }

        public UsuarioOutputDto BuscarUsuarioLogado()
        {
            Usuario usuarioLogado = _usuarioLogadoService.ObterUsuario();
            return EntidadeParaDto(usuarioLogado);
        }

        public UsuarioOutputDto Cadastrar(UsuarioInputDto dto)
        {
            if (_contexto.Usuarios.Any(u=>u.Email==dto.Email))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "já existe um usuário cadastrado com esse email");
            }
            string hashSenha=_senhaService.GerarHashDaSenha(dto.Senha);
            Usuario usuario = new(_geradorCodigoIndentificador.GerarCodigoUsuario(),dto.Nome, dto.Email, hashSenha,dto.PerfilUsuario);
            _repositorio.Cadastrar(usuario);
            _usuarioLogService.CriarLogDeCriacao(usuario, _usuarioLogadoService.ObterUsuario());
            _contexto.SaveChanges();
            return EntidadeParaDto(usuario);
        }

        public UsuarioOutputDto Atualizar(Guid id, UsuarioInputDto dto)
        {
            Usuario usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(u => u.Id == id)
               ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            if (usuarioVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Usuário esta inativo, reative-o antes para poder atualiza-lo");
            }

            Usuario? usuarioVindoDoBancoComEmailInformado = _contexto.Usuarios.FirstOrDefault(u=>u.Email==dto.Email);

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

        public UsuarioOutputDto AtualizarUsuarioLogado(AlteracaoDeUsuarioLogadoDto dto)
        {
            Usuario usuarioLogado = _usuarioLogadoService.ObterUsuario();


            if (!_senhaService.ValidarSenha(usuarioLogado.Senha, dto.Senha))
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Senha incorreta");
            }

            Usuario? usuarioVindoDoBancoComEmailInformado = _contexto.Usuarios.FirstOrDefault(u=>u.Email==dto.Email);

            if (usuarioVindoDoBancoComEmailInformado != null && usuarioVindoDoBancoComEmailInformado.Id != usuarioLogado.Id)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Já existe um usuário com esse email cadastrado");
            }
            Usuario usuarioCopia = usuarioLogado.Copia();
            usuarioLogado.Nome = dto.Nome;
            usuarioLogado.Email = dto.Email;
            if (dto.SenhaNova != null)
            {
                usuarioLogado.Senha = _senhaService.GerarHashDaSenha(dto.SenhaNova);
            }
            _repositorio.Atualizar(usuarioLogado);
            _usuarioLogService.CriarLogsDeAtualizacao(usuarioCopia, usuarioLogado, usuarioLogado);
            _contexto.SaveChanges();
            return EntidadeParaDto(usuarioLogado);

        }

        public void Inativar(Guid id)
        {
            Usuario usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(u => u.Id == id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            if (usuarioVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400,"Usuário já está inativo");
            }

            if (usuarioVindoDoBanco.PerfilUsuario == PerfilUsuario.Admin)
            {
                int quantidadeDeUsuariosAdminAtivo = _contexto.Usuarios.Count(u => u.PerfilUsuario == PerfilUsuario.Admin && u.Ativo);
                if (quantidadeDeUsuariosAdminAtivo == 1)
                {
                    throw new ExcecaoDeRegraDeNegocio(400, "não é possível inativar esse usuário admin, pois o sistema precisa ter um usuário admin ativo");

                }
            }

            usuarioVindoDoBanco.Ativo = false;
            _contexto.Usuarios.Update(usuarioVindoDoBanco);
            _usuarioLogService.CriarLogsDeInativacao(usuarioVindoDoBanco, _usuarioLogadoService.ObterUsuario());
            _contexto.SaveChanges();
        }

        public void Reativar(Guid id)
        {
            Usuario usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(u => u.Id == id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            if (usuarioVindoDoBanco.Ativo == true)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "Usuário já está ativo");
            }

            usuarioVindoDoBanco.Ativo = true;
            _contexto.Usuarios.Update(usuarioVindoDoBanco);
            _usuarioLogService.CriarLogsDeReativacao(usuarioVindoDoBanco, _usuarioLogadoService.ObterUsuario());
            _contexto.SaveChanges();
        }

        public string Login(UsuarioLoginDto dto)
        {
            Usuario usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(u=>u.Email==dto.Email)
                ??throw new ExcecaoDeRegraDeNegocio(400, "usuário ou senha inválida");
            if (usuarioVindoDoBanco.Ativo == false)
            {
                throw new ExcecaoDeRegraDeNegocio(400, "usuário está inativo, entre em contato com o administrador do sistema, para reativa-lo, para fazer login");
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
            Usuario usuario = _contexto.Usuarios.FirstOrDefault(u => u.Id == id)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            List<UsuarioLog> logs = _contexto.UsuarioLogs.Include(l=>l.Usuario)
                .Where(l => l.IdUsuario == usuario.Id).ToList();

            List<UsuarioLogOutputDto> logsDto =
                logs.Select(l => new UsuarioLogOutputDto
                (l.IdUsuario,
                l.Usuario.CodigoUsuario,
                l.Acao,
                l.CampoAlterado,
                l.ValorAntigo,
                l.ValorNovo,
                l.IdUsuarioResponsavel,
                l.DataCriacao)).ToList();

            return logsDto.OrderByDescending(l => l.DataCriacao).ToList();
        }

        public List<UsuarioLogOutputDto> BuscarLogsPorCodigoUsuario(string codigoUsuario)
        {
            Usuario usuario = _contexto.Usuarios.FirstOrDefault(u => u.CodigoUsuario == codigoUsuario)
                ?? throw new ExcecaoDeRegraDeNegocio(404, "Usuário não encontrado");

            List<UsuarioLog> logs = _contexto.UsuarioLogs.Include(l => l.Usuario)
                .Where(l => l.IdUsuario == usuario.Id).ToList();

            List<UsuarioLogOutputDto> logsDto =
                logs.Select(l => new UsuarioLogOutputDto
                (l.IdUsuario,
                l.Usuario.CodigoUsuario,
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
            return new UsuarioOutputDto(usuario.Id, usuario.DataCriacao, usuario.Ativo,usuario.CodigoUsuario, usuario.Nome, usuario.Email,usuario.PerfilUsuario);
        }

    }
}
