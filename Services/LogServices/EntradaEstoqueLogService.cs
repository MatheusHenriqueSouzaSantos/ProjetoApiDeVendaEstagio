using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class EntradaEstoqueLogService
    {
        private readonly LogRepositorio<EntradaEstoqueLog> _repositorio;

        public EntradaEstoqueLogService(LogRepositorio<EntradaEstoqueLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(EntradaEstoque entradaEstoque, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(EntradaEstoque).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(entradaEstoque);

                EntradaEstoqueLog log = new(entradaEstoque,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(EntradaEstoque entradaEstoqueAntiga, EntradaEstoque entradaEstoqueAtualizada, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(EntradaEstoque).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(entradaEstoqueAntiga);
                var valorAtualizadoPropriedade = propriedade.GetValue(entradaEstoqueAtualizada);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    EntradaEstoqueLog log = new(
                        entradaEstoqueAtualizada,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(EntradaEstoque entradaEstoque,StatusEntradaEstoque statusAnterior, Usuario usuarioResponsavel)
        {
            EntradaEstoqueLog log = new(entradaEstoque,
                LogAcao.Inativacao,
                "Ativo",
                "true",
                "false",
                usuarioResponsavel);
            _repositorio.CriarLog(log);
            log = new(entradaEstoque,
                LogAcao.Inativacao,
                "Status",
                statusAnterior.ToString(),
                StatusEntradaEstoque.Cancelada.ToString(),
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }
    }
}
