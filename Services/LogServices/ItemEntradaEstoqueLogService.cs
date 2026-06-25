using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices
{
    public class ItemEntradaEstoqueLogService
    {
        private readonly LogRepositorio<ItemEntradaEstoqueLog> _repositorio;

        public ItemEntradaEstoqueLogService(LogRepositorio<ItemEntradaEstoqueLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public void CriarLogsDeCriacao(ItemEntradaEstoque itemEntradaEstoque,EntradaEstoque entradaEstoqueDoItem, Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ItemEntradaEstoque).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogCriacao)))
                {
                    continue;
                }
                var valorPropriedade = propriedade.GetValue(itemEntradaEstoque);

                ItemEntradaEstoqueLog log = new(itemEntradaEstoque,
                    entradaEstoqueDoItem,
                    LogAcao.Criacao,
                    propriedade.Name,
                    null,
                    valorPropriedade?.ToString(),
                    usuarioResponsavel);

                _repositorio.CriarLog(log);
            }
        }

        public void CriarLogsDeAtualizacao(ItemEntradaEstoque itemEntradaEstoqueAntigo, ItemEntradaEstoque itemEntradaEstoqueAtualizado,
            EntradaEstoque entradaEstoqueDoItem,Usuario usuarioResponsavel)
        {
            PropertyInfo[] propriedades = typeof(ItemEntradaEstoque).GetProperties();
            foreach (PropertyInfo propriedade in propriedades)
            {
                if (Attribute.IsDefined(propriedade, typeof(AtributoASerIgnoradoLogAtualizacao)))
                {
                    continue;
                }
                var valorAntigoPropriedade = propriedade.GetValue(itemEntradaEstoqueAntigo);
                var valorAtualizadoPropriedade = propriedade.GetValue(itemEntradaEstoqueAtualizado);

                if (!Equals(valorAntigoPropriedade, valorAtualizadoPropriedade))
                {
                    ItemEntradaEstoqueLog log = new(
                        itemEntradaEstoqueAtualizado,
                        entradaEstoqueDoItem,
                        LogAcao.Atualizacao,
                        propriedade.Name,
                        valorAntigoPropriedade?.ToString(),
                        valorAtualizadoPropriedade?.ToString(),
                        usuarioResponsavel);

                    _repositorio.CriarLog(log);
                }

            }
        }

        public void CriarLogsDeExclusao(ItemEntradaEstoque itemEntradaEstoque,EntradaEstoque entradaEstoqueDoItem, Usuario usuarioResponsavel)
        {
            ItemEntradaEstoqueLog log = new(itemEntradaEstoque,
                entradaEstoqueDoItem,
                LogAcao.Exclusao,
                "Ativo",
                "true",
                "false",
                usuarioResponsavel);

            _repositorio.CriarLog(log);

        }
    }
}

