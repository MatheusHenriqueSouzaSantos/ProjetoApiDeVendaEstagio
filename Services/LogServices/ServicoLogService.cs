using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ServicoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class ServicoLogService
    {
        private readonly LogRepositorio<ServicoLog> _repositorio;

        public ServicoLogService(LogRepositorio<ServicoLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Servico servico, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Servico).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(servico);

                ServicoLog log = new(servico,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Servico servicoAntigo, Servico servicoAtualizado, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Servico).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AnotacaoDeAtributoASerIgnoradoLog)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(servicoAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(servicoAtualizado);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    ServicoLog log = new(servicoAtualizado,
                    LogAcao.Atualizacao,
                    propriedade.Name,
                    valorAntigoPropriedade?.ToString(),
                    valorAtualizadoPropriedade?.ToString(),
                    usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Servico servico, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new ServicoLog(servico, LogAcao.Exclusao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel));
        }
    }
}
