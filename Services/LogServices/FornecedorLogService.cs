using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class FornecedorLogService
    {
        private readonly LogRepositorio<FornecedorLog> _repositorio;

        public FornecedorLogService(LogRepositorio<FornecedorLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(Fornecedor fornecedor, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Fornecedor).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                var valorPropriedade = propriedade.GetValue(fornecedor);

                FornecedorLog log = new(fornecedor,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(Fornecedor fornecedorAntigo, Fornecedor fornecedorAtualizado, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(Vendedor).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AnotacaoDeAtributoASerIgnoradoLog)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(fornecedorAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(fornecedorAtualizado);

                if (valorAntigoPropriedade != valorAtualizadoPropriedade)
                {
                    FornecedorLog log = new(fornecedorAtualizado,
                    LogAcao.Atualizacao,
                    propriedade.Name,
                    valorAntigoPropriedade?.ToString(),
                    valorAtualizadoPropriedade?.ToString(),
                    usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(Fornecedor fornecedor, Usuario usuarioResponsavel)
        {
            _repositorio.CriarLog(new FornecedorLog(fornecedor, LogAcao.Exclusao, "Ativo", true.ToString(), false.ToString(), usuarioResponsavel));
        }
    }
}
