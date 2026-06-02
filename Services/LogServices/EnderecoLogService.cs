using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.ClienteDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class EnderecoLogService
    {
        private readonly LogRepositorio<EnderecoLog> _repositorio;

        public EnderecoLogService(LogRepositorio<EnderecoLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Endereco endereco,Cliente cliente, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Endereco).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(endereco);

                EnderecoLog log = new(endereco,
                    cliente,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Endereco enderecoAntigo, Endereco enderecoAtualizado, Cliente cliente, 
            Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Endereco).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AnotacaoDeAtributoASerIgnoradoLog)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(enderecoAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(enderecoAtualizado);

                if (valorAntigoPropriedade != valorAtualizadoPropriedade)
                {
                    EnderecoLog log = 
                    new(enderecoAtualizado,
                    cliente,
                    LogAcao.Atualizacao,
                    propriedade.Name,
                    valorAntigoPropriedade?.ToString(),
                    valorAtualizadoPropriedade?.ToString(),
                    usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Endereco endereco, Cliente cliente, Usuario usuarioResponsavel)
        {
            EnderecoLog log = new EnderecoLog(endereco,cliente, LogAcao.Exclusao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel);

            _repositorio.CriarLog(log);
        }
    }
}
