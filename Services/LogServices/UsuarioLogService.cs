using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class UsuarioLogService
    {
        private readonly LogRepositorio<UsuarioLog> _repositorio;

        public UsuarioLogService(LogRepositorio<UsuarioLog> repositorio)
        {
            _repositorio = repositorio;
        }
        public void CriarLogDeCriacao(Usuario usuario, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Usuario).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(usuario);

                UsuarioLog log = new(usuario,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Usuario usuarioAntigo, Usuario usuarioAtualizado, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Usuario).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(usuarioAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(usuarioAtualizado);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    UsuarioLog log = new(
                        usuarioAtualizado,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Usuario usuario, Usuario usuarioResponsavel)
        {
            UsuarioLog log = new(usuario, LogAcao.Inativacao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel);

            _repositorio.CriarLog(log);
        }
    }
}
